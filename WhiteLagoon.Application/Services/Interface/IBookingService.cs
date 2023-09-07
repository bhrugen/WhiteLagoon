using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IBookingService
    {
        Booking GetBookingById(int bookingId);

        void CreateBooking(Booking booking);

        void UpdateBooking(Booking booking);

        void CancelBooking(int bookingId);

        void CheckInBooking(int bookingId);

        void CheckOutBooking(int bookingId);

        IEnumerable<Booking> GetBookingsByUserId(string userId);

        IEnumerable<Booking> GetAllBookings(string? status);

        double CalculateTotalCost(int villaId, DateOnly checkInDate, int nights);

        IEnumerable<Booking> GetBookedVillasWithStatus(IEnumerable<string> statuses);

        void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId);

        void UpdateStatus(int bookingId, string status, int villaNumber);

        IEnumerable<int> GetCheckedInVillaNumbers(int villaId);

    }
}
