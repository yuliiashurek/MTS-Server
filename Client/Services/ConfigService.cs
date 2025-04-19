using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class ConfigService
{
    public static void SaveEncrypted<T>(string filePath, T value)
    {
        var json = JsonSerializer.Serialize(value);
        var encrypted = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(json),
            null,
            DataProtectionScope.CurrentUser);

        var base64 = Convert.ToBase64String(encrypted);
        File.WriteAllText(filePath, base64);
    }

    public static T? LoadEncrypted<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return default;

        try
        {
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
    }
}
