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
            var totalBookings = _unitOfWork.Booking.GetAll();
            
            var countByCurrentMonth = totalBookings.Count(r => r.BookingDate >= currentMonthStartDate && r.BookingDate < DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(r => r.BookingDate >= previousMonthStartDate && r.BookingDate < currentMonthStartDate);



            return Json();
        }
        
    }
}
