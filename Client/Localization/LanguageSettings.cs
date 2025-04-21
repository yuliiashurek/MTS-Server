using System.Globalization;
using System.Windows;
using System.Diagnostics;
using Client.Properties;

namespace Client
{
    public static class LanguageSettings
    {
        public static string Language => Settings.Default.Language;
        private const string _defaultLang = "uk";
        public static void ApplySavedCulture()
        {
            var cultureCode = Settings.Default.Language ?? _defaultLang;
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static void ChangeCulture(string cultureCode)
        {
            Settings.Default.Language = cultureCode;
            Settings.Default.Save();

            if (SessionManager.Current != null)
            {
                SessionManager.SetSession(SessionManager.Current);
            }

            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var exePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true
            });
            Application.Current.Shutdown();
        }

    }
}
