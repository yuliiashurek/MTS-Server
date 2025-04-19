using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Services
{
    using Server.Core.Interfaces;
    using Server.Data.UnitOfWork;
    using Server.Shared.DTOs;
    using Server.Data.Entities;
    using System.Security.Cryptography;
    using System.Text;

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public UserService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<List<UserDto>> GetAllInOrganizationAsync(Guid adminId)
        {
            var admin = await _unitOfWork.Users.GetByIdAsync(adminId);
            if (admin == null || admin.Role != "Admin")
                return [];

            var users = await _unitOfWork.Users.GetAllByOrganizationIdAsync(admin.OrganizationId);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role,
                HasPassword = u.PasswordHash != null
            }).ToList();
        }

        public async Task<bool> CreateUserAsync(Guid adminId, CreateUserDto dto)
        {
            var admin = await _unitOfWork.Users.GetByIdAsync(adminId);
            if (admin == null || admin.Role != "Admin")
                return false;

            var user = new User
            {
                Email = dto.Email,
                Role = dto.Role ?? "Employee",
                OrganizationId = admin.OrganizationId,
                PasswordResetToken = Guid.NewGuid().ToString(),
                PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(24)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid adminId, Guid userId)
        {
            var admin = await _unitOfWork.Users.GetByIdAsync(adminId);
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (admin == null || user == null || admin.OrganizationId != user.OrganizationId)
                return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendInviteAsync(Guid adminId, Guid userIdToInvite)
        {
            var admin = await _unitOfWork.Users.GetByIdAsync(adminId);
            var user = await _unitOfWork.Users.GetByIdAsync(userIdToInvite);
            if (admin == null || user == null || admin.OrganizationId != user.OrganizationId)
                return false;

            // Генеруємо новий токен
            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(24);

            await _unitOfWork.SaveChangesAsync();

            var resetUrl = $"https://localhost:7299/set-password.html?token={user.PasswordResetToken}";
            await _emailService.SendAsync(user.Email, "Запрошення до системи", $"Встановіть пароль: {resetUrl}");

            return true;
        }
    }

}
