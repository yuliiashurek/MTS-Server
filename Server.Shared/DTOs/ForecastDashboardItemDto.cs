namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для відображення інформації у дашборді прогнозу витрат матеріалів.
    /// </summary>
    public class ForecastDashboardItemDto
    {
        /// <summary>
        /// Ідентифікатор матеріалу.
        /// </summary>
        public Guid MaterialId { get; set; }

        /// <summary>
        /// Назва матеріалу.
        /// </summary>
        public string MaterialName { get; set; } = string.Empty;

        /// <summary>
        /// Одиниця вимірювання матеріалу.
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Назва категорії, до якої належить матеріал.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Назва складу, де зберігається матеріал.
        /// </summary>
        public string Warehouse { get; set; } = string.Empty;

        /// <summary>
        /// Поточний залишок матеріалу.
        /// </summary>
        public double CurrentStock { get; set; }

        /// <summary>
        /// Середнє денне споживання матеріалу.
        /// </summary>
        public double AverageDailyUsage { get; set; }

        /// <summary>
        /// Прогнозоване споживання на 7 днів.
        /// </summary>
        public double Forecast7Days { get; set; }

        /// <summary>
        /// Мінімально допустимий залишок.
        /// </summary>
        public double MinStock { get; set; }

        /// <summary>
        /// Кількість днів, на які вистачить залишку.
        /// </summary>
        public int DaysLeft { get; set; }

        /// <summary>
        /// Рекомендація щодо закупівлі або контролю.
        /// </summary>
        public string Recommendation { get; set; } = string.Empty;

        /// <summary>
        /// Чи є залишок критичним.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Чи є залишок попереджувальним (на межі).
        /// </summary>
        public bool IsWarning { get; set; }

        /// <summary>
        /// Чи залишок у нормі.
        /// </summary>
        public bool IsNormal { get; set; }
    }
}
