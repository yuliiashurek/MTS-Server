using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Client.Helpers
{
    public static class TokenStorage
    {
        private static readonly byte[] entropy = Encoding.UTF8.GetBytes("SomeEntropy123!");

        public static void Save(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ConfigurationManager.AppSettings.Set(key, null);
                return;
            }

            var encrypted = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(value),
                entropy,
                DataProtectionScope.CurrentUser);

            var base64 = Convert.ToBase64String(encrypted);
            Properties.Settings.Default[key] = base64;
            Properties.Settings.Default.Save();
        }

        public static string? Load(string key)
        {
            var base64 = Properties.Settings.Default[key]?.ToString();
            if (string.IsNullOrEmpty(base64)) return null;

            var encrypted = Convert.FromBase64String(base64);
            var decrypted = ProtectedData.Unprotect(
                encrypted,
                entropy,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decrypted);
        }

        public static void Clear(string key)
        {
            Properties.Settings.Default[key] = null;
            Properties.Settings.Default.Save();
        }
    }
}
