using Client;
using Server.Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserSession?> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Username = email, Password = password });
        if (!response.IsSuccessStatusCode) return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return new UserSession
        {
            AccessToken = result!.Token,
            RefreshToken = result.RefreshToken,
            Role = result.Role,
            Email = email
        };
    }

    public async Task<string?> RefreshTokenAsync(string refreshToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", refreshToken);
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        return content?.Token;
    }

    public void AddAuthHeader()
    {
        if (SessionManager.Current != null)
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Current.AccessToken);
    }

    public async Task<bool> RegisterOrganizationAsync(RegisterOrganizationDto dto)
    {
        var response = await App.HttpClient.PostAsJsonAsync("api/auth/register-organization", dto);
        return response.IsSuccessStatusCode;
    }

}
