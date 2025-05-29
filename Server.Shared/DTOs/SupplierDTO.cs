namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для постачальника матеріалів.
    /// </summary>
    public class SupplierDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор постачальника.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва постачальника.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Контактна особа постачальника.
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Номер телефону постачальника.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email постачальника.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Адреса постачальника.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Код ЄДРПОУ постачальника.
        /// </summary>
        public string EdrpouCode { get; set; } = string.Empty;
    }
}
