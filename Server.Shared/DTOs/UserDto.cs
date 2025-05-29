namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для представлення користувача в системі.
    /// </summary>
    public class UserDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор користувача.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Email користувача.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Роль користувача (наприклад, Admin або Employee).
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Ознака, чи користувач уже встановив пароль.
        /// </summary>
        public bool HasPassword { get; set; }
    }
}
