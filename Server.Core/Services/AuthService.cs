using Microsoft.Extensions.Configuration;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IConfiguration config,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Users.FindByUsernameAsync(request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
            int.Parse(_config["Jwt:RefreshTokenExpirationDays"]!));

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            Role = user.Role
        };
    }

    public async Task<string?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _unitOfWork.Users.FindByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return null;

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        //var newRefreshToken = _tokenService.GenerateRefreshToken();

        //user.RefreshToken = newRefreshToken;
        //user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
        //    int.Parse(_config["Jwt:RefreshTokenExpirationDays"]!));

        //_unitOfWork.Users.Update(user);
        // await _unitOfWork.SaveChangesAsync();

        return newAccessToken;
    }

    public async Task<bool> RegisterOrganizationAsync(RegisterOrganizationDto dto)
    {
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = dto.OrganizationName,
            Users = new List<User>()
        };

        //var admin = new User
        //{
        //    Id = Guid.NewGuid(),
        //    Email = dto.AdminEmail,
        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.AdminPassword),
        //    Organization = organization,
        //    Role = "Admin"
        //};

        await _unitOfWork.Organizations.AddAsync(organization);


        var token = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.AdminEmail,
            OrganizationId = organization.Id,
            Role = "Admin",
            PasswordResetToken = token,
            PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(24)
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var resetUrl = $"https://localhost:7299/set-password.html?token={token}";
        await _emailService.SendAsync(dto.AdminEmail, "Ваш акаунт у системі", $"Перейдіть за посиланням: {resetUrl}");

        //await _unitOfWork.Users.AddAsync(admin);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> InviteUserAsync(Guid inviterId, CreateUserDto dto)
    {
        var inviter = await _unitOfWork.Users.GetByIdAsync(inviterId);
        if (inviter == null)
            return false;

        var token = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            OrganizationId = inviter.OrganizationId,
            Role = dto.Role,
            PasswordResetToken = token,
            PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(24)
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var resetUrl = $"https://localhost:7299/set-password.html?token={token}";
        await _emailService.SendAsync(dto.Email, "Ваш акаунт у системі", $"Перейдіть за посиланням: {resetUrl}");

        return true;
    }

    public async Task<bool> SetPasswordAsync(SetPasswordDto dto)
    {
        var user = await _unitOfWork.Users.FindByPasswordResetTokenAsync(dto.Token);
        if (user == null || user.PasswordResetTokenExpiryTime < DateTime.UtcNow)
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiryTime = null;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
