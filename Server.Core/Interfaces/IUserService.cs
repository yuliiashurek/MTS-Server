using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{

    public interface IUserService
    {
        Task<List<UserDto>> GetAllInOrganizationAsync(Guid adminId);
        Task<bool> CreateUserAsync(Guid adminId, CreateUserDto dto);
        Task<bool> DeleteUserAsync(Guid adminId, Guid userId);
        Task<bool> SendInviteAsync(Guid adminId, Guid userIdToInvite);
    }

}
