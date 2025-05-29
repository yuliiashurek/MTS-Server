namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO, що представляє значення залишку матеріалу на певну дату.
    /// </summary>
    public class StockHistoryPointDto
    {
        /// <summary>
        /// Дата, на яку зафіксовано залишок.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Кількість залишку на вказану дату.
        /// </summary>
        public decimal Stock { get; set; }
    }
}
