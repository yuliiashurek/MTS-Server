namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для одиниці вимірювання матеріалів.
    /// </summary>
    public class MeasurementUnitDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор одиниці вимірювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Повна назва одиниці вимірювання (наприклад, "кілограм").
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Скорочена назва одиниці (наприклад, "кг").
        /// </summary>
        public string ShortName { get; set; } = string.Empty;
    }
}
