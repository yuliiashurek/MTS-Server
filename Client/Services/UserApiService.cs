using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Services
{

    public class UserApiService
    {
        private readonly HttpClient _http;

        public UserApiService()
        {
            _http = new HttpClient(new AuthHttpClientHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri("https://localhost:7262/api/")
            };
        }

        public async Task<List<UserDto>?> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<UserDto>>("users");
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var response = await _http.DeleteAsync($"users/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InviteAsync(Guid userId)
        {
            var response = await _http.PostAsJsonAsync("users/invite", userId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateAsync(CreateUserDto dto)
        {
            var response = await _http.PostAsJsonAsync("Users", dto);
            return response.IsSuccessStatusCode;
        }
    }

}
