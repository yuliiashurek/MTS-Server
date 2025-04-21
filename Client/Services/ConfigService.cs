using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Client {
    public static class ConfigService
    {
        public static void SaveEncrypted<T>(string filePath, T value)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                var json = JsonSerializer.Serialize(value);
                var encrypted = ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(json),
                    null,
                    DataProtectionScope.CurrentUser);

                var base64 = Convert.ToBase64String(encrypted);
                File.WriteAllText(filePath, base64);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalUICulture;
            }
        }

        public static T? LoadEncrypted<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default;

            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                var base64 = File.ReadAllText(filePath);
                var encrypted = Convert.FromBase64String(base64);
                var decrypted = ProtectedData.Unprotect(
                    encrypted, null, DataProtectionScope.CurrentUser);

                var json = Encoding.UTF8.GetString(decrypted);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalUICulture;
            }
        }
    }
}
