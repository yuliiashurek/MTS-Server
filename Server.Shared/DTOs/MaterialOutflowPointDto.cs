namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO, що представляє обсяг списання матеріалу за конкретну дату.
    /// </summary>
    public class MaterialOutflowPointDto
    {
        /// <summary>
        /// Дата списання матеріалу.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Кількість списаного матеріалу на цю дату.
        /// </summary>
        public decimal OutQuantity { get; set; }
    }
}
