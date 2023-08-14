using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

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

        public static RadialBarChartDto GetRadialCartDataModel(int totalCount, double currentMonthCount, double prevMonthCount)
        {
            RadialBarChartDto RadialBarChartDto = new();


            int increaseDecreaseRatio = 100;

            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
            }

            RadialBarChartDto.TotalCount = totalCount;
            RadialBarChartDto.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            RadialBarChartDto.HasRatioIncreased = currentMonthCount > prevMonthCount;
            RadialBarChartDto.Series = new int[] { increaseDecreaseRatio };

            return RadialBarChartDto;
        }
    }
}
