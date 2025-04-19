using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Client.Helpers;

public class AuthHttpClientHandler : DelegatingHandler
{
    private readonly AuthApiService _authService;

    public AuthHttpClientHandler(HttpMessageHandler innerHandler)
        : base(innerHandler) 
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7262/")
        };
        _authService = new (httpClient);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(SessionManager.Current?.AccessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Current.AccessToken);

        var response = await base.SendAsync(request, cancellationToken);

        // Якщо токен не валідний — пробуємо оновити
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(SessionManager.Current.RefreshToken))
        {
            var newToken = await _authService.RefreshTokenAsync(SessionManager.Current.RefreshToken);
            if (!string.IsNullOrEmpty(newToken))
            {
                SessionManager.Current.AccessToken = newToken;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                response = await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }
}
