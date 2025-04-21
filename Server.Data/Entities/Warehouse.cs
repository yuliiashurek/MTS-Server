namespace Server.Data.Entities
{
    public class Warehouse : IOrganizationOwnedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
    }
}
