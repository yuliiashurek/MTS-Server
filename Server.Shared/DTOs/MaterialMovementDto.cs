namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для операцій руху матеріалів (надходження або списання).
    /// </summary>
    public class MaterialMovementDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор операції.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ідентифікатор організації, якій належить операція.
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Ідентифікатор матеріалу, до якого належить операція.
        /// </summary>
        public Guid MaterialItemId { get; set; }

        /// <summary>
        /// Ідентифікатор складу, з якого або на який відбувається рух.
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Тип руху матеріалу: 0 — надходження, 1 — списання.
        /// </summary>
        public int MovementType { get; set; }

        /// <summary>
        /// Кількість одиниць матеріалу.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Ціна за одиницю матеріалу.
        /// </summary>
        public decimal PricePerUnit { get; set; }

        /// <summary>
        /// Дата руху матеріалу (операції).
        /// </summary>
        public DateTime MovementDate { get; set; }

        /// <summary>
        /// Термін придатності матеріалу (необов’язкове поле).
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Номер штрихкоду (якщо є).
        /// </summary>
        public string BarcodeNumber { get; set; }

        /// <summary>
        /// Ідентифікатор одержувача у випадку списання (необов’язкове поле).
        /// </summary>
        public Guid? RecipientId { get; set; }
    }
}
