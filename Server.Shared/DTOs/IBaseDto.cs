namespace Server.Shared.DTOs
{
    /// <summary>
    /// Базовий інтерфейс для DTO, що містять ідентифікатор.
    /// </summary>
    public interface IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор об'єкта.
        /// </summary>
        Guid Id { get; set; }
    }
}
