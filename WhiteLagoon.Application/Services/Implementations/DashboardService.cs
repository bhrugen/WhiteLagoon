using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interfaces;
using WhiteLagoon.Shared.ViewModels;

namespace WhiteLagoon.Application.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<DashboardPieChartVM> GetBookingPieChartDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RadialBarChartVM> GetBookingsChartDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DashboardLineChartVM> GetMemberAndBookingChartDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RadialBarChartVM> GetRegisteredUserChartDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RadialBarChartVM> GetRevenueChartDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
