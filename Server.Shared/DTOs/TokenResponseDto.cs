namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для відповіді з access токеном після авторизації або оновлення токена.
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// JWT access токен.
        /// </summary>
        public string Token { get; set; }
    }
}
