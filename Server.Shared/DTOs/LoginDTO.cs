namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для запиту авторизації користувача.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Ім’я користувача або email.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль користувача.
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// DTO для відповіді після успішної авторизації.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT токен доступу.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Токен оновлення (refresh token).
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Роль користувача (наприклад, Admin або Employee).
        /// </summary>
        public string Role { get; set; }
    }
}
