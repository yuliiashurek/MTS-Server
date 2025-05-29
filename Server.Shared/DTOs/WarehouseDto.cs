namespace Server.Shared.DTOs
{
    /// <summary>
    /// DTO для представлення складу.
    /// </summary>
    public class WarehouseDto : IBaseDto
    {
        /// <summary>
        /// Унікальний ідентифікатор складу.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва складу.
        /// </summary>
        public string Name { get; set; }
    }
}
