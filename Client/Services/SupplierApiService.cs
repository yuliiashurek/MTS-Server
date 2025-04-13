using Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Services
{
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
            return await _httpClient.GetFromJsonAsync<List<Supplier>>("suppliers");
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _httpClient.PostAsJsonAsync("suppliers", supplier);
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            await _httpClient.PutAsJsonAsync($"suppliers/{supplier.Id}", supplier);
        }

        public async Task DeleteSupplierAsync(int id)
        {
            await _httpClient.DeleteAsync($"suppliers/{id}");
        }
    }
}
