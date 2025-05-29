namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для категорії матеріалів.
    /// </summary>
    public class CategoryDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор категорії.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва категорії.
        /// </summary>
        public string Name { get; set; }
    }
}
