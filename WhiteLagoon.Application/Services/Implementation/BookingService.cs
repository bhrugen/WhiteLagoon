using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
	public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "User,Villa");
        }

        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public void UpdateBooking(Booking booking)
        {
            _unitOfWork.Booking.Update(booking);
            _unitOfWork.Save();
        }

        public void CancelBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking != null)
            {
                booking.Status = SD.StatusCancelled;
                UpdateBooking(booking);
            }
        }

        public void CheckInBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking != null)
            {
                booking.Status = SD.StatusCheckedIn;
                UpdateBooking(booking);
            }
        }

        public void CheckOutBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking != null)
            {
                booking.Status = SD.StatusCompleted;
                UpdateBooking(booking);
            }
        }

        public IEnumerable<Booking> GetBookingsByUserId(string userId)
        {
            return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User, Villa");
        }

        public IEnumerable<Booking> GetAllBookings(string? status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                return _unitOfWork.Booking.GetAll(u => !string.IsNullOrEmpty(u.Status) && u.Status.ToLower().Equals(status.ToLower()), includeProperties: "User, Villa");
            }

            return _unitOfWork.Booking.GetAll(includeProperties: "User, Villa");
        }

        public double CalculateTotalCost(int villaId, DateOnly checkInDate, int nights)
        {
            var villa = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (villa != null)
            {
                return villa.Price * nights;
            }
            return 0;
        }

        public IEnumerable<Booking> GetBookedVillasWithStatus(IEnumerable<string> statuses)
        {
            return _unitOfWork.Booking.GetAll(u => statuses.Contains(u.Status));
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == bookingId);

            if (booking != null)
            {
                booking.StripeSessionId = sessionId;
                booking.StripePaymentIntentId = paymentIntentId;
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<int> GetCheckedInVillaNumbers(int villaId)
        {
            return _unitOfWork.Booking
                .GetAll(u => u.VillaId == villaId && u.Status == SD.StatusCheckedIn)
                .Select(u => u.VillaNumber);
        }
        public void UpdateStatus(int bookingId, string status, int villaNumber)
        {
            // Get the booking from the database
            var booking = _unitOfWork.Booking.Get(u => u.Id == bookingId);

            if (booking != null)
            {
                // Update the booking status
                booking.Status = status;

                if (villaNumber > 0)
                {
                    // Update the villa number if it's provided
                    booking.VillaNumber = villaNumber;
                }

                // Save the changes
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save(); // Assuming you have an async Save method
            }
        }

    }

}
