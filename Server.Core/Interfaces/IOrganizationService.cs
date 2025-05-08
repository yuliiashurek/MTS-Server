using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{
    public interface IOrganizationService
    {
        Task<OrganizationInfoDto?> GetMyOrganizationAsync(Guid userId);
        Task<bool> UpdateMyOrganizationAsync(Guid userId, OrganizationInfoDto dto);
    }

}
