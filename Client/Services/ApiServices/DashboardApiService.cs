using Client.Models;
using Server.Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

public class DashboardApiService
{
    private readonly HttpClient _httpClient;

    public DashboardApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MaterialCurrentStockDto>> GetCurrentStocksAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<MaterialCurrentStockDto>>("dashboard/current-stocks");
        return result ?? new List<MaterialCurrentStockDto>();
    }

    public async Task<List<StockHistoryPointDto>> GetStockHistoryAsync(Guid materialId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<StockHistoryPointDto>>($"dashboard/{materialId}/stock-history");
        return result ?? new List<StockHistoryPointDto>();
    }

    public async Task<List<ForecastDashboardItemDto>> GetForecastAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ForecastDashboardItemDto>>("dashboard/forecast");
        return result ?? new List<ForecastDashboardItemDto>();
    }

    public async Task<List<MaterialOutflowPointDto>> GetMaterialOutflowHistoryAsync(Guid materialId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<MaterialOutflowPointDto>>(
            $"dashboard/forecast/{materialId}/outflow");
        return result ?? new List<MaterialOutflowPointDto>();
    }


}
