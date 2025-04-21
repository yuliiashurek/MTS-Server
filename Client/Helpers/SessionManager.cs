using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Client
{
    public static class SessionManager
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user.config");

        public static UserSession? Current { get; private set; }

        public static void SetSession(UserSession session)
        {
            Current = session;
            ConfigService.SaveEncrypted(FilePath, session);
        }

        public static void ClearSession()
        {
            Current = null;
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        public static void LoadSession()
        {
            Current = ConfigService.LoadEncrypted<UserSession>(FilePath);
        }

        public static bool IsLoggedIn => Current != null;

        public static bool IsJwtExpired(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return true;

            var payload = parts[1];
            var padded = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(padded));
            var exp = JsonDocument.Parse(json).RootElement.GetProperty("exp").GetInt64();

            var expireTime = DateTimeOffset.FromUnixTimeSeconds(exp);
            return expireTime < DateTimeOffset.UtcNow;
        }

        public static async Task<UserSession?> TryRestoreSessionAsync()
        {
            LoadSession();
            var session = Current;
            if (session == null) return null;

            if (!IsJwtExpired(session.AccessToken))
            {
                SetSession(session);
                App.SharedHttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.AccessToken);
                return session;
            }

            var newAccessToken = await AuthApiService.RefreshTokenAsync(session.RefreshToken);
            if (string.IsNullOrEmpty(newAccessToken))
            {
                ClearSession();
                try { File.Delete(FilePath); } catch { }
                return null;
            }

            session.AccessToken = newAccessToken;
            SetSession(session);
            App.SharedHttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newAccessToken);
            return session;
        }
    }
}
