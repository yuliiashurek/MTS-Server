namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для опису матеріалу, що обліковується в системі.
    /// </summary>
    public class MaterialItemDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор матеріалу.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ідентифікатор організації, якій належить матеріал.
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Назва матеріалу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ідентифікатор одиниці вимірювання.
        /// </summary>
        public Guid MeasurementUnitId { get; set; }

        /// <summary>
        /// Мінімально допустимий залишок на складі.
        /// </summary>
        public decimal MinimumStock { get; set; }

        /// <summary>
        /// Ідентифікатор категорії матеріалу.
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Ідентифікатор постачальника.
        /// </summary>
        public Guid SupplierId { get; set; }

        /// <summary>
        /// Email-адреси для сповіщень про критичний рівень залишку (через кому).
        /// </summary>
        public string NotificationEmails { get; set; } = string.Empty;
    }
}
