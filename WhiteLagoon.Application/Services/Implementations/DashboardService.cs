using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.DTO;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interfaces;

namespace WhiteLagoon.Application.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardPieChartDTO> GetBookingPieChartDataAsync()
        {
            DashboardPieChartDTO DashboardPieChartDTO = new();
            try
            {
                var newCustomerBookings = _unitOfWork.Booking.GetAll().AsEnumerable().GroupBy(b => b.UserId)
                    .Where(g => g.Count() == 1).Select(g => g.Key).Count();

                var returningCustomerBookings = _unitOfWork.Booking.GetAll().AsEnumerable().GroupBy(b => b.UserId)
                    .Where(g => g.Count() > 1).Select(g => g.Key).Count();

                DashboardPieChartDTO.Labels = new string[] { "New Customers", "Returning Customers" };
                DashboardPieChartDTO.Series = new decimal[] { newCustomerBookings, returningCustomerBookings };

            }
            catch (Exception ex)
            {
                throw;
            }
            return DashboardPieChartDTO;
        }

        public async Task<RadialBarChartDTO> GetBookingsChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll().ToList();

            var countByCurrentMonth = totalBookings.Count(r => r.BookingDate >= currentMonthStartDate && r.BookingDate < DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(r => r.BookingDate >= previousMonthStartDate && r.BookingDate < currentMonthStartDate);
            return SD.GetRadialChartDataModel(totalBookings.Count, countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<DashboardLineChartDTO> GetMemberAndBookingChartDataAsync()
        {
            DashboardLineChartDTO DashboardLineChartDTO = new();
            try
            {
                // Query for new bookings and new customers
                var bookingData = _unitOfWork.Booking.GetAll()
                    .Where(b => b.BookingDate.Date >= DateTime.Now.AddDays(-30) && b.BookingDate.Date <= DateTime.Now)
                    .GroupBy(b => b.BookingDate.Date)
                    .Select(g => new
                    {
                        DateTime = g.Key,
                        NewBookingCount = g.Count()
                    })
                    .ToList();

                var customerData = _unitOfWork.User.GetAll()
                    .Where(u => u.CreatedAt.Date >= DateTime.Now.AddDays(-30) && u.CreatedAt.Date <= DateTime.Now)
                    .GroupBy(u => u.CreatedAt.Date)
                    .Select(g => new
                    {
                        DateTime = g.Key,
                        NewCustomerCount = g.Count()
                    })
                    .ToList();

                // Perform a left outer join
                var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                    (booking, customer) => new
                    {
                        booking.DateTime,
                        booking.NewBookingCount,
                        NewCustomerCount = customer.Select(b => b.NewCustomerCount).SingleOrDefault()
                    })
                    .ToList();


                // Perform a right outer join
                var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
                    (customer, bookings) => new
                    {
                        customer.DateTime,
                        NewBookingCount = bookings.Select(b => b.NewBookingCount).SingleOrDefault(),
                        customer.NewCustomerCount
                    })
                    .Where(x => x.NewBookingCount == 0).ToList();

                // Combine the left and right joins
                var mergedData = leftJoin.Union(rightJoin).OrderBy(data => data.DateTime).ToList();

                // Separate the counts into individual lists
                var newBookingData = mergedData.Select(d => d.NewBookingCount).ToList();
                var newCustomerData = mergedData.Select(d => d.NewCustomerCount).ToList();
                var categories = mergedData.Select(d => d.DateTime.Date.ToString("MM/dd/yyyy")).ToList();


                List<ChartData> chartDataList = new List<ChartData>
                {
                    new ChartData { Name = "New Memebers", Data = newCustomerData.ToArray() },
                    new ChartData { Name = "New Bookings", Data = newBookingData.ToArray() }
                };

                DashboardLineChartDTO.Series = chartDataList;
                DashboardLineChartDTO.Categories = categories.ToArray();

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw;
            }

            return DashboardLineChartDTO;
        }

        public async Task<RadialBarChartDTO> GetRegisteredUserChartDataAsync()
        {
            var totalUsers = _unitOfWork.User.GetAll().ToList();
            var countByCurrentMonth = totalUsers.Count(r => r.CreatedAt >= currentMonthStartDate && r.CreatedAt < DateTime.Now);
            var countByPreviousMonth = totalUsers.Count(r => r.CreatedAt >= previousMonthStartDate && r.CreatedAt < currentMonthStartDate);
            return SD.GetRadialChartDataModel(totalUsers.Count, countByCurrentMonth, countByPreviousMonth);

        }

        public async Task<RadialBarChartDTO> GetRevenueChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll().ToList();
            var sumByCurrentMonth = totalBookings.Where((r => r.BookingDate >= currentMonthStartDate && r.BookingDate < DateTime.Now)).Sum(x => x.TotalCost);
            var sumByPreviousMonth = totalBookings.Where(r => r.BookingDate >= previousMonthStartDate && r.BookingDate < currentMonthStartDate).Sum(x => x.TotalCost);
            return SD.GetRadialChartDataModel(Convert.ToDecimal(totalBookings.Sum(x => x.TotalCost)), sumByCurrentMonth, sumByPreviousMonth);

        }
    }
}
