using System.IO;

public static class SessionManager
{
    private static readonly string FilePath = "user.config";

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
}
