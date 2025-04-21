namespace Server.Core.Interfaces
{
    public interface ISessionService
    {
        Guid OrganizationId { get; }
        Guid UserId { get; }
        string Role { get; }
    }
}
