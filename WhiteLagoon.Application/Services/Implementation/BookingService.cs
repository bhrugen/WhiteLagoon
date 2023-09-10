using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
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

        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBookings(string userId = "", string? statusFilterList = "")
        {
            IEnumerable<string> statusList = statusFilterList.ToLower().Split(",");
            if(!string.IsNullOrEmpty(statusFilterList) && !string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) &&
                u.UserId == userId, includeProperties: "User,Villa");
            }
            else
            {
                if (!string.IsNullOrEmpty(statusFilterList))
                {
                    return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()), includeProperties: "User,Villa");
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User,Villa");
                }
            }
            return _unitOfWork.Booking.GetAll(includeProperties: "User, Villa");
        }

        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(u=>u.Id==bookingId,includeProperties: "User, Villa");
        }
    }
}
