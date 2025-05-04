using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.DTOs
{
    public class ForecastDashboardItemDto
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
        public double CurrentStock { get; set; }
        public double AverageDailyUsage { get; set; }
        public double Forecast7Days { get; set; }
        public double MinStock { get; set; }
        public int DaysLeft { get; set; }
        public string Recommendation { get; set; } = string.Empty;
        public bool IsCritical { get; set; }
        public bool IsWarning { get; set; }
        public bool IsNormal { get; set; }

    }

}
