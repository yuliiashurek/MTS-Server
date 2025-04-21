using Server.Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        private const string _users = "users";

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>?> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>(_users);
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var response = await _httpClient.DeleteAsync($"{_users}/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InviteAsync(Guid userId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_users}/invite", userId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateAsync(CreateUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(_users, dto);
            return response.IsSuccessStatusCode;
        }
    }

}
