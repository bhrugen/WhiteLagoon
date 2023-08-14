using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.DTO;
namespace WhiteLagoon.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<RadialBarChartDTO> GetBookingsChartDataAsync();
        Task<RadialBarChartDTO> GetRevenueChartDataAsync();
        Task<RadialBarChartDTO> GetRegisteredUserChartDataAsync();
        Task<DashboardLineChartDTO> GetMemberAndBookingChartDataAsync();
        Task<DashboardPieChartDTO> GetBookingPieChartDataAsync();
    }
}
