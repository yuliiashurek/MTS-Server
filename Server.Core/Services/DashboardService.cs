using Server.Core.Interfaces;
using Server.Data;
using Server.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Server.Data.Db;
using Server.Shared.Enums;
using System.Globalization;

namespace Server.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaterialCurrentStockDto>> GetCurrentStocksAsync(Guid organizationId)
        {
            var stocks = await _context.MaterialItems
                .Where(m => m.OrganizationId == organizationId)
                .Select(m => new MaterialCurrentStockDto
                {
                    MaterialItemId = m.Id,
                    MaterialName = m.Name,
                    UnitName = m.MeasurementUnit.ShortName,
                    CategoryName = m.Category.Name,
                    WarehouseName = "", // заглушка
                    CurrentStock = _context.MaterialMovements
                        .Where(mm => mm.MaterialItemId == m.Id)
                        .Sum(mm => mm.MovementType == 0 ? mm.Quantity : -mm.Quantity),
                    MinimumStock = m.MinimumStock
                })
                .ToListAsync();

            return stocks;
        }

        public async Task<List<StockHistoryPointDto>> GetStockHistoryAsync(Guid materialItemId, Guid organizationId)
        {
            var movements = await _context.MaterialMovements
                .Where(m => m.MaterialItemId == materialItemId && m.OrganizationId == organizationId)
                .OrderBy(m => m.MovementDate)
                .ToListAsync();

            decimal stock = 0;
            var history = new List<StockHistoryPointDto>();

            foreach (var movement in movements)
            {
                stock += movement.MovementType == 0 ? movement.Quantity : -movement.Quantity;
                history.Add(new StockHistoryPointDto
                {
                    Date = movement.MovementDate,
                    Stock = stock
                });
            }

            return history;
        }

        public async Task<List<ForecastDashboardItemDto>> GetForecastDashboardItemsAsync(Guid organizationId)
        {
            var materials = await _context.MaterialItems
                .Include(m => m.MeasurementUnit)
                .Include(m => m.Category)
                .Where(m => m.OrganizationId == organizationId)
                .ToListAsync();

            var result = new List<ForecastDashboardItemDto>();

            foreach (var material in materials)
            {
                var movements = await _context.MaterialMovements
                    .Where(m => m.MaterialItemId == material.Id)
                    .ToListAsync();

                var inQty = movements
                    .Where(m => m.MovementType == (int)MovementType.In)
                    .Sum(m => m.Quantity);

                var fourWeeksAgoMonday = DateTime.Today
                    .StartOfWeek(DayOfWeek.Monday)
                    .AddDays(-7 * 3); 

                var outMovements = movements
                    .Where(m =>
                        m.MovementType == (int)MovementType.Out &&
                        m.MovementDate.Date >= fourWeeksAgoMonday)
                    .OrderByDescending(m => m.MovementDate)
                    .ToList();

                var outQty = movements
                    .Where(m => m.MovementType == (int)MovementType.Out)
                    .Sum(m => m.Quantity);

                var currentStock = inQty - outQty;

                var startDate = DateTime.Today.StartOfWeek(DayOfWeek.Monday).AddDays(-7 * 3);
                var endDate = DateTime.Today.StartOfWeek(DayOfWeek.Monday).AddDays(5);       

                // обмежуємо діапазон
                var relevantMovements = movements
                    .Where(m => m.MovementType == (int)MovementType.Out &&
                                m.MovementDate.Date >= startDate &&
                                m.MovementDate.Date < endDate)
                    .ToList();

                // підраховуємо загальну кількість
                var totalOut = relevantMovements.Sum(m => m.Quantity);

                // 4 тижні × 5 робочих днів = 20 днів
                var avgDailyUsage = totalOut / 20;
                var forecast7Days = avgDailyUsage * 5;
                var daysLeft = avgDailyUsage > 0 
                    ? (int)Math.Floor(currentStock / avgDailyUsage) 
                    : currentStock > 0 ? 9999 : 0;

                var recommendation = currentStock < forecast7Days
                    ? "Замовити терміново"
                    : (currentStock < material.MinimumStock ? "Скоро замовити" : "Не потрібно");

                result.Add(new ForecastDashboardItemDto
                {
                    MaterialId = material.Id,
                    MaterialName = material.Name,
                    Unit = material.MeasurementUnit.ShortName,
                    Category = material.Category.Name,
                    Warehouse = "-", // заглушка
                    CurrentStock = (double)currentStock,
                    AverageDailyUsage = Math.Round((double)avgDailyUsage, 2),
                    Forecast7Days = Math.Round((double)forecast7Days, 2),
                    MinStock = (double)material.MinimumStock,
                    DaysLeft = daysLeft,
                    Recommendation = recommendation
                });
            }

            return result;
        }

        public async Task<List<MaterialOutflowPointDto>> GetMaterialOutflowHistoryAsync(Guid materialId, Guid organizationId)
        {
            var startDate = DateTime.Today.AddDays(-30);
            var raw = await _context.MaterialMovements
                .Where(m => m.MaterialItemId == materialId &&
                            m.OrganizationId == organizationId &&
                            m.MovementType == (int)MovementType.Out &&
                            m.MovementDate >= startDate)
                .ToListAsync();

            var grouped = raw
                .GroupBy(x => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                    x.MovementDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new MaterialOutflowPointDto
                {
                    Date = g.Min(x => x.MovementDate.Date),
                    OutQuantity = g.Sum(x => x.Quantity)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return grouped;
        }


    }
    public static class DateExtensions
    {
        public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
    }

}
