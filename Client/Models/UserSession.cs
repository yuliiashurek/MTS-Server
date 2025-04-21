namespace Client
{
    public class UserSession
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}

