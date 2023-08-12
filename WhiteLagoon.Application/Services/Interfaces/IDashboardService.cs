using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Shared.ViewModels;

namespace WhiteLagoon.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<RadialBarChartVM> GetBookingsChartDataAsync();
        Task<RadialBarChartVM> GetRevenueChartDataAsync();
        Task<RadialBarChartVM> GetRegisteredUserChartDataAsync();
        Task<DashboardLineChartVM> GetMemberAndBookingChartDataAsync();
        Task<DashboardPieChartVM> GetBookingPieChartDataAsync();
    }
}
