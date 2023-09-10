using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IBookingService
    {
        void CreateBooking(Booking booking);
        Booking GetBookingById(int bookingId);
        IEnumerable<Booking> GetAllBookings(string userId="",string? statusFilterList="");

        void UpdateStatus(int bookingId, string bookingStatus, int villaNumber);
        void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId);

        public IEnumerable<int> GetCheckedInVillaNumbers(int villaId);
    }
}
