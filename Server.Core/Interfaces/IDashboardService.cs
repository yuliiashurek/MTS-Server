using Server.Shared.DTOs;

namespace Server.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<List<MaterialCurrentStockDto>> GetCurrentStocksAsync(Guid organizationId);
        Task<List<StockHistoryPointDto>> GetStockHistoryAsync(Guid materialItemId, Guid organizationId);
        Task<List<ForecastDashboardItemDto>> GetForecastDashboardItemsAsync(Guid organizationId);
        Task<List<MaterialOutflowPointDto>> GetMaterialOutflowHistoryAsync(Guid materialId, Guid organizationId);


    }
}
