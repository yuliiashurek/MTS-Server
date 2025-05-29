namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для одержувача матеріалів (контрагента).
    /// </summary>
    public class RecipientDto
    {
        /// <summary>
        /// Унікальний ідентифікатор одержувача.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва одержувача.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Код ЄДРПОУ одержувача.
        /// </summary>
        public string Edrpou { get; set; } = "";

        /// <summary>
        /// Адреса одержувача.
        /// </summary>
        public string Address { get; set; } = "";

        /// <summary>
        /// Контактна особа одержувача.
        /// </summary>
        public string ContactPerson { get; set; } = "";
    }
}
