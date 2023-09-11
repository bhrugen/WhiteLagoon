using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.Security.Claims;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Implementation;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
	public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IVillaService _villaService;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IPaymentService _paymentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookingController(IBookingService bookingService, IUserService userService, 
            IVillaService villaService, IVillaNumberService villaNumberService, IPaymentService paymentService,
            IWebHostEnvironment webHostEnvironment)
		{
            _villaService = villaService;
            _villaNumberService = villaNumberService;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var user = await _userService.GetUserById(userId);

            var villa = _villaService.GetVillaById(villaId);

            if (villa == null)
            {
                // Handle the case where the villa does not exist.
                return RedirectToAction("Error", "Home");
            }

			var booking = new Booking
			{
				VillaId = villaId,
				Villa = villa,
				CheckInDate = checkInDate,
				Nights = nights,
				CheckOutDate = checkInDate.AddDays(nights),
				UserId = userId,
				Phone = user.PhoneNumber,
				Email = user.Email ?? string.Empty,
				Name = user.Name,
				TotalCost = villa.Price * nights
			};

			return View(booking);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FinalizeBooking(Booking booking)
        {
            var villa = _villaService.GetVillaById(booking.VillaId);

            if (villa == null)
            {
                // Handle the case where the villa does not exist.
                return RedirectToAction("Error", "Home");
            }

            booking.TotalCost = villa.Price * booking.Nights;
            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            var villaNumbersList = _villaNumberService.GetAllVillaNumbers().ToList();

            var bookedVillas = _bookingService.GetBookedVillasWithStatus(new[] { SD.StatusApproved, SD.StatusCheckedIn }).ToList();

            int roomAvailable = _villaService.GetAvailableRoomCount(villa.Id, villaNumbersList, booking.CheckInDate, booking.Nights, bookedVillas);

            if (roomAvailable == 0)
            {
                TempData["error"] = "Room has been sold out!";
                // No rooms available
                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    villaId = booking.VillaId,
                    checkInDate = booking.CheckInDate,
                    nights = booking.Nights
                });
            }

            _bookingService.CreateBooking(booking);

            var domain = $"{Request.Scheme}://{Request.Host.Value}/";
            
            var options = _paymentService.CreateSessionOptions(booking, villa, domain);

            var session = await _paymentService.CreateSessionAsync(options);

            _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);

            if (bookingFromDb.Status == SD.StatusPending)
            {
                //this is a pending order, we need to confirm if payment was successful

                var service = new SessionService();
                Session session = service.Get(bookingFromDb.StripeSessionId);

                if (session.PaymentStatus == "paid")
                {
                    _bookingService.UpdateStatus(bookingFromDb.Id, SD.StatusApproved,0);
                    _bookingService.UpdateStripePaymentID(bookingFromDb.Id,session.Id,session.PaymentIntentId);
                }
            }

            return View(bookingId);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);

            if(bookingFromDb.VillaNumber==0 && bookingFromDb.Status == SD.StatusApproved)
            {
                var availableVillaNumber = AssignAvailableVillaNumberByVilla(bookingFromDb.VillaId);

                var villaNumbers = _villaNumberService.GetVillaNumbersByAvailability(bookingFromDb.VillaId, availableVillaNumber);
                bookingFromDb.VillaNumbers = villaNumbers;
            }

            return View(bookingFromDb);
        }
               
        [HttpPost]
        [Authorize]
        public IActionResult GenerateInvoice(int id, string downloadType)
        {
            var stream = _bookingService.GenerateInvoiceStream(id, downloadType);

            if (downloadType == "word")
            {
                return File(stream, "application/docx", "BookingDetails.docx");
            }
            else
            {
                return File(stream, "application/pdf", "BookingDetails.pdf");
            }
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckIn(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);
            
            TempData["Success"] = "Booking Updated Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckOut(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted , booking.VillaNumber);
            
            TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, 0);

            TempData["Success"] = "Booking Cancelled Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }


        private List<int> AssignAvailableVillaNumberByVilla(int villaId)
        {
            List<int> availableVillaNumbers = new();

            var villaNumbers = _villaNumberService.GetAllVillaNumbersByVillaId(villaId);

            var checkedInVilla = _bookingService.GetCheckedInVillaNumbers(villaId);    

            foreach(var villaNumber in villaNumbers)
            {
                if (!checkedInVilla.Contains(villaNumber.Villa_Number))
                {
                    availableVillaNumbers.Add(villaNumber.Villa_Number);
                }
            }
            return availableVillaNumbers;
        }


        #region API Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> objBookings; 

            if (User.IsInRole(SD.Role_Admin))
            {
                objBookings = _bookingService.GetAllBookings(status);
            }
            else
            {
                //var claimsIdentity = (ClaimsIdentity)User.Identity;
                //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                objBookings = _bookingService.GetBookingsByUserId(userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                objBookings  = objBookings.Where(u => u.Status.ToLower().Equals(status.ToLower()));
            }
            return Json(new { data = objBookings });
        }

        #endregion
    }
}
