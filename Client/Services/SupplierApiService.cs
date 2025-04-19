using Server.Shared.DTOs;
using Client.Models;
using System.Net.Http.Json;
using Client;
using Client.Helpers;
using System.Net.Http.Headers;
using System.Net.Http;

public class SupplierApiService
{
    private readonly HttpClient _httpClient;
    private const string _suppliers = "suppliers";

    public SupplierApiService()
    {
        _httpClient = new HttpClient(new AuthHttpClientHandler(new HttpClientHandler()))
        {
            BaseAddress = new Uri("https://localhost:7262/api/")
        };
    }

    public async Task<List<Supplier>> GetSuppliersAsync()
    {
        var dtos = await _httpClient.GetFromJsonAsync<List<SupplierDto>>(_suppliers);
        return dtos.Select(dto => App.Mapper.Map<Supplier>(dto)).ToList();
    }

    public async Task AddSupplierAsync(Supplier supplier)
    {
        var dto = App.Mapper.Map<SupplierDto>(supplier);
        await _httpClient.PostAsJsonAsync(_suppliers, dto);
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        var dto = App.Mapper.Map<SupplierDto>(supplier);
        await _httpClient.PutAsJsonAsync($"{_suppliers}/{supplier.Id}", dto);
    }

    public async Task DeleteSupplierAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"{_suppliers}/{id}");
    }
}
