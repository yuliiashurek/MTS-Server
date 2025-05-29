namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для відображення поточних залишків матеріалів на складі.
    /// </summary>
    public class MaterialCurrentStockDto
    {
        /// <summary>
        /// Ідентифікатор матеріалу.
        /// </summary>
        public Guid MaterialItemId { get; set; }

        /// <summary>
        /// Назва матеріалу.
        /// </summary>
        public string MaterialName { get; set; } = string.Empty;

        /// <summary>
        /// Назва одиниці вимірювання.
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// Назва категорії матеріалу.
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Назва складу, на якому зберігається матеріал.
        /// </summary>
        public string WarehouseName { get; set; } = string.Empty;

        /// <summary>
        /// Поточна кількість матеріалу на складі.
        /// </summary>
        public decimal CurrentStock { get; set; }

        /// <summary>
        /// Мінімально допустима кількість залишку.
        /// </summary>
        public decimal MinimumStock { get; set; }

        /// <summary>
        /// Чи залишок є критичним (менше або дорівнює мінімуму).
        /// </summary>
        public bool IsCritical => CurrentStock <= MinimumStock;

        /// <summary>
        /// Чи залишок на рівні попередження (близький до критичного).
        /// </summary>
        public bool IsWarning =>
            !IsCritical && CurrentStock <= MinimumStock * (decimal)1.1;

        /// <summary>
        /// Чи залишок у нормі.
        /// </summary>
        public bool IsNormal =>
            !IsCritical && !IsWarning;
    }
}
