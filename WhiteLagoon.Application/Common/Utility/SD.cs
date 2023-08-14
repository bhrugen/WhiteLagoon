using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.DTO;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int VillaRoomsAvailable_Count(int villaId, 
            List<VillaNumber> villaNumberList, DateOnly checkInDate, int nights,
           List<Booking> bookings)
        {
            List<int> bookingInDate = new();
            int finalAvailableRoomForAllNights = int.MaxValue;
            var roomsInVilla = villaNumberList.Where(x => x.VillaId == villaId).Count();

            for(int i = 0; i < nights; i++)
            {
                var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
                && u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);

                foreach(var booking in villasBooked)
                {
                    if (!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }

                var totalAvailableRooms = roomsInVilla - bookingInDate.Count;
                if(totalAvailableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if(finalAvailableRoomForAllNights > totalAvailableRooms)
                    {
                        finalAvailableRoomForAllNights = totalAvailableRooms;
                    }
                }
            }

            return finalAvailableRoomForAllNights;
        }

        public static RadialBarChartDTO GetRadialChartDataModel(decimal total, double currentMonthCount, double prevMonthCount)
        {
            RadialBarChartDTO dashboardRadialBarChartDTO = new();
            decimal increaseDecreaseRatio = 100;
            bool isIncrease = true;

            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToDecimal(Math.Round(((double)currentMonthCount - prevMonthCount) / prevMonthCount * 100, 2));
                isIncrease = currentMonthCount > prevMonthCount;
            }

            dashboardRadialBarChartDTO.TotalCount = total;
            dashboardRadialBarChartDTO.IncreaseDecreaseAmount = (decimal)currentMonthCount;
            dashboardRadialBarChartDTO.IncreaseDecreaseRatio = increaseDecreaseRatio;
            dashboardRadialBarChartDTO.HasRatioIncreased = isIncrease;
            dashboardRadialBarChartDTO.Series = new decimal[] { increaseDecreaseRatio };
            return dashboardRadialBarChartDTO;
        }
    }
}
