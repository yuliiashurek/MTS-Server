// Server.Data/Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using Server.Data.Db;
using Server.Data.Entities;
using Server.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<User?> FindByUsernameAsync(string email)
        => _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> FindByRefreshTokenAsync(string token)
        => _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);

    public Task<User?> FindByPasswordResetTokenAsync(string token)
        => _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token);

    public async Task<List<User>> GetAllByOrganizationIdAsync(Guid organizationId)
    {
        return await _context.Users
            .Where(u => u.OrganizationId == organizationId)
            .ToListAsync();
    }

}

