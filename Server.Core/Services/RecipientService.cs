using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Services
{
    public class RecipientService : IRecipientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ISessionService _session;

        public RecipientService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _session = session;
        }

        public async Task<RecipientDto?> GetByNameAsync(string name)
        {
            var entity = await _unitOfWork.Recipients.GetByNameAsync(_session.OrganizationId, name);
            return entity == null ? null : _mapper.Map<RecipientDto>(entity);
        }

        public async Task<RecipientDto> CreateAsync(RecipientDto dto)
        {
            var entity = _mapper.Map<Recipient>(dto);
            entity.OrganizationId = _session.OrganizationId;
            entity.Id = Guid.NewGuid();

            await _unitOfWork.Recipients.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RecipientDto>(entity);
        }
    }

}
