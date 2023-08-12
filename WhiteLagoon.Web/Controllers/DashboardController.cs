using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Implementations;
using WhiteLagoon.Application.Services.Interfaces;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingsChartData()
        {
            return Json(await _dashboardService.GetBookingsChartDataAsync());
        }

        public async Task<IActionResult> GetTotalRevenueChartData()
        {
            return Json(await _dashboardService.GetRevenueChartDataAsync());
        }

        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            return Json(await _dashboardService.GetRegisteredUserChartDataAsync());
        }

        public async Task<IActionResult> GetCustomerBookingsPieChartData()
        {
            return Json(await _dashboardService.GetBookingPieChartDataAsync());
        }

        public async Task<IActionResult> GetMemberAndBookingChartData()
        {
            return Json(await _dashboardService.GetMemberAndBookingChartDataAsync());
        }
    }
}
