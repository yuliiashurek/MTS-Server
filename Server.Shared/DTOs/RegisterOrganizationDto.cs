namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для реєстрації нової організації разом із першим адміністратором.
    /// </summary>
    public class RegisterOrganizationDto
    {
        /// <summary>
        /// Назва організації.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Email адміністратора організації.
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Пароль для облікового запису адміністратора.
        /// </summary>
        public string AdminPassword { get; set; }
    }
}
