using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.DTO
{
    public class DashboardPieChartDTO
    {
        public decimal[] Series { get; set; }
        public string[] Labels { get; set; }
    }
}
