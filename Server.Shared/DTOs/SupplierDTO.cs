namespace Server.Shared.DTOs
{
    public class SupplierDto : IBaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string EdrpouCode { get; set; } = string.Empty;
    }
}
