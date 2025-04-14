using Server.Shared.DTOs;
using Client.Models;
using System.Net.Http.Json;
using Client;
using System.Net.Http;

public class SupplierApiService
{
    private readonly HttpClient _httpClient;

    public SupplierApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7262/api/")
        };
    }

    public async Task<List<Supplier>> GetSuppliersAsync()
    {
        var dtos = await _httpClient.GetFromJsonAsync<List<SupplierDto>>("suppliers");
        return dtos.Select(dto => App.Mapper.Map<Supplier>(dto)).ToList();
    }

    public async Task AddSupplierAsync(Supplier supplier)
    {
        var dto = App.Mapper.Map<SupplierDto>(supplier);
        await _httpClient.PostAsJsonAsync("suppliers", dto);
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        var dto = App.Mapper.Map<SupplierDto>(supplier);
        await _httpClient.PutAsJsonAsync($"suppliers/{supplier.Id}", dto);
    }

    public async Task DeleteSupplierAsync(int id)
    {
        await _httpClient.DeleteAsync($"suppliers/{id}");
    }
}
