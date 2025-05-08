using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Core.Interfaces;
using Server.Data.Db;
using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrganizationInfoDto?> GetMyOrganizationAsync(Guid userId)
        {
            var orgId = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.OrganizationId)
                .FirstOrDefaultAsync();

            var org = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == orgId);

            return org is null ? null : _mapper.Map<OrganizationInfoDto>(org);
        }

        public async Task<bool> UpdateMyOrganizationAsync(Guid userId, OrganizationInfoDto dto)
        {
            var orgId = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.OrganizationId)
                .FirstOrDefaultAsync();

            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.Id == orgId);
            if (org is null)
                return false;

            _mapper.Map(dto, org);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
