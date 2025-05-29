namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO з основною інформацією про організацію.
    /// </summary>
    public class OrganizationInfoDto
    {
        /// <summary>
        /// Коротка назва організації.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код ЄДРПОУ організації.
        /// </summary>
        public string EdrpouCode { get; set; } = string.Empty;

        /// <summary>
        /// Повна назва організації.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Адреса організації.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Місто для документів (відображається в актах).
        /// </summary>
        public string CityForDocs { get; set; } = string.Empty;

        /// <summary>
        /// ПІБ особи, що підписує документи від організації.
        /// </summary>
        public string FioForDocs { get; set; } = string.Empty;
    }
}
