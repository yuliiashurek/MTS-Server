using System;

namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для генерації акта приймання матеріалів.
    /// </summary>
    public class AcceptanceActRequestDto
    {
        /// <summary>
        /// Ідентифікатор постачальника.
        /// </summary>
        public Guid SupplierId { get; set; }

        /// <summary>
        /// Початкова дата періоду, за який формується акт.
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Кінцева дата періоду, за який формується акт.
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Номер договору, якщо вказано.
        /// </summary>
        public string? ContractNumber { get; set; }
    }
}
