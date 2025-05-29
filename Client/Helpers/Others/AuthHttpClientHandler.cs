using Server.Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;

namespace Client
{
    public class AuthHttpClientHandler : DelegatingHandler
    {
        public AuthHttpClientHandler(HttpMessageHandler innerHandler)
            : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(SessionManager.Current?.AccessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Current.AccessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(SessionManager.Current?.RefreshToken))
            {
                var tempClient = new HttpClient { BaseAddress = App.SharedHttpClient.BaseAddress };
                var refreshResponse = await tempClient.PostAsJsonAsync("auth/refresh", SessionManager.Current.RefreshToken);

                if (refreshResponse.IsSuccessStatusCode)
                {
                    var newToken = (await refreshResponse.Content.ReadFromJsonAsync<TokenResponseDto>())?.Token;
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        SessionManager.Current.AccessToken = newToken;
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                        return await base.SendAsync(request, cancellationToken);
                    }
                }
            }

            return response;
        }
    }
}