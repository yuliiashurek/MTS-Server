namespace Server.Data.Entities
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
    }
}
