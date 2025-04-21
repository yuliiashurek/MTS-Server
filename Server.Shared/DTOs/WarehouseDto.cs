namespace Server.Shared.DTOs
{
    public class WarehouseDto : IBaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
