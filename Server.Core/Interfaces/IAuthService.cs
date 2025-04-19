using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto dto);
        Task<string?> RefreshTokenAsync(string refreshToken);
        Task<bool> RegisterOrganizationAsync(RegisterOrganizationDto dto);
        Task<bool> InviteUserAsync(Guid inviterId, CreateUserDto dto);
        Task<bool> SetPasswordAsync(SetPasswordDto dto);
    }


}
