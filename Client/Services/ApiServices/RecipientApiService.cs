using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services.ApiServices
{
    public class RecipientApiService
    {
        private readonly HttpClient _http = App.SharedHttpClient;

        public async Task<RecipientDto?> GetByNameAsync(string name)
        {
            var response = await _http.GetAsync($"recipients/by-name/{name}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return null;

            return JsonSerializer.Deserialize<RecipientDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }


        public async Task<RecipientDto> CreateAsync(RecipientDto dto)
        {
            var response = await _http.PostAsJsonAsync("recipients", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<RecipientDto>();
        }
    }

}
