using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Services.ApiServices
{
    public class ReportsApiService
    {
        private readonly HttpClient _httpClient;

        public ReportsApiService()
        {
            _httpClient = App.SharedHttpClient;
        }

        public async Task<byte[]?> GenerateAcceptanceActAsync(Guid supplierId, DateTime dateFrom, DateTime dateTo, string? contractNumber)
        {
            var request = GetRequest(supplierId, dateFrom, dateTo, contractNumber);

            var response = await _httpClient.PostAsJsonAsync("reports/generate-acceptance-act", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }

        public async Task<string?> GetAcceptanceActHtmlPreviewAsync(Guid supplierId, DateTime dateFrom, DateTime dateTo, string? contractNumber)
        {
            var request = GetRequest(supplierId, dateFrom, dateTo, contractNumber);

            var response = await _httpClient.PostAsJsonAsync("reports/generate-html-preview", request);
            return await response.Content.ReadAsStringAsync();
        }

        private object GetRequest(Guid supplierId, DateTime dateFrom, DateTime dateTo, string? contractNumber)
        {
            var request = new
            {
                SupplierId = supplierId,
                DateFrom = dateFrom,
                DateTo = dateTo,
                ContractNumber = contractNumber
            };
            return request;
        }
    }
}
