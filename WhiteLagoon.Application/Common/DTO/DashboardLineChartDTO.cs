using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.DTO
{
    public class DashboardLineChartDTO
    {
        public List<ChartData> Series { get; set; }
        public string[] Categories { get; set; }
    }

    public class ChartData
    {
        public string Name { get; set; }
        public int[] Data { get; set; }
    }
}
