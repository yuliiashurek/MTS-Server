namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для генерації акта передачі матеріалів.
    /// </summary>
    public class TransferActRequestDto
    {
        /// <summary>
        /// Ідентифікатор одержувача, якому передаються матеріали.
        /// </summary>
        public Guid RecipientId { get; set; }

        /// <summary>
        /// Початкова дата періоду, за який формується акт.
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Кінцева дата періоду, за який формується акт.
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Номер договору, якщо є.
        /// </summary>
        public string? ContractNumber { get; set; }
    }
}
