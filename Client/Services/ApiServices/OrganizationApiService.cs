using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.ApiServices
{
    public class OrganizationApiService
    {
        private readonly HttpClient _httpClient = App.SharedHttpClient;

        public async Task<Organization?> GetMyOrganizationAsync()
        {
            return await _httpClient.GetFromJsonAsync<Organization>("organization/me");
        }

        public async Task<bool> UpdateMyOrganizationAsync(Organization organization)
        {
            var response = await _httpClient.PutAsJsonAsync("organization/me", organization);
            return response.IsSuccessStatusCode;
        }
    }
}
