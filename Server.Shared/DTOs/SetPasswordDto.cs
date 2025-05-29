namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для встановлення пароля користувачем за отриманим токеном запрошення.
    /// </summary>
    public class SetPasswordDto
    {
        /// <summary>
        /// Токен, отриманий у листі-запрошенні (ідентифікує користувача).
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Новий пароль, який встановлює користувач.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
