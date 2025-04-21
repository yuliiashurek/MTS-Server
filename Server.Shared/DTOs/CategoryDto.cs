namespace Server.Shared.DTOs
{
    public class CategoryDto : IBaseDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
