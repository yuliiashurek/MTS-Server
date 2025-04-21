using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByUsernameAsync(string email);
    Task<User?> FindByRefreshTokenAsync(string refreshToken);
    Task<User?> FindByPasswordResetTokenAsync(string token);
    Task<List<User>> GetAllByOrganizationIdAsync(Guid organizationId);

}

