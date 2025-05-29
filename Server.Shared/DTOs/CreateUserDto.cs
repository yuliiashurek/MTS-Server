namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для створення нового користувача в організації.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Email користувача, на який буде надіслано запрошення.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Роль користувача (наприклад, "Admin" або "Employee").
        /// </summary>
        public string Role { get; set; }
    }
}
