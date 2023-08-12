using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingsChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll().ToList();
            
            var countByCurrentMonth = totalBookings.Count(r => r.BookingDate >= currentMonthStartDate && r.BookingDate < DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(r => r.BookingDate >= previousMonthStartDate && r.BookingDate < currentMonthStartDate);

            return Json(GetRadialChartDataModel(totalBookings.Count,countByCurrentMonth,countByPreviousMonth));
        }

        public async Task<IActionResult> GetTotalRevenueChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll().ToList();
            var sumByCurrentMonth = totalBookings.Where((r => r.BookingDate >= currentMonthStartDate && r.BookingDate < DateTime.Now)).Sum(x => x.TotalCost);
            var sumByPreviousMonth = totalBookings.Where(r => r.BookingDate >= previousMonthStartDate && r.BookingDate < currentMonthStartDate).Sum(x => x.TotalCost);
            return Json(GetRadialChartDataModel(Convert.ToDecimal(totalBookings.Sum(x => x.TotalCost)), sumByCurrentMonth, sumByPreviousMonth));

        }

        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            var totalUsers = _unitOfWork.User.GetAll().ToList();
            var countByCurrentMonth = totalUsers.Count(r => r.CreatedAt >= currentMonthStartDate && r.CreatedAt < DateTime.Now);
            var countByPreviousMonth = totalUsers.Count(r => r.CreatedAt >= previousMonthStartDate && r.CreatedAt < currentMonthStartDate);
            return Json(GetRadialChartDataModel(totalUsers.Count, countByCurrentMonth, countByPreviousMonth));
        }




        private RadialBarChartVM GetRadialChartDataModel(decimal total, double currentMonthCount, double prevMonthCount)
        {
            RadialBarChartVM dashboardRadialBarChartVM = new();
            decimal increaseDecreaseRatio = 100;
            bool isIncrease = true;

            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToDecimal(Math.Round(((double)currentMonthCount - prevMonthCount) / prevMonthCount * 100, 2));
                isIncrease = currentMonthCount > prevMonthCount;
            }

            dashboardRadialBarChartVM.TotalCount = total;
            dashboardRadialBarChartVM.IncreaseDecreaseAmount = (decimal)currentMonthCount;
            dashboardRadialBarChartVM.IncreaseDecreaseRatio = increaseDecreaseRatio;
            dashboardRadialBarChartVM.HasRatioIncreased = isIncrease;
            dashboardRadialBarChartVM.Series = new decimal[] { increaseDecreaseRatio };
            return dashboardRadialBarChartVM;
        }
    }
}
