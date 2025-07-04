﻿using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;

namespace Server.Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService sessionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sessionService = sessionService;
        }

        public async Task<List<SupplierDto>> GetAllAsync()
        {
            var orgId = _sessionService.OrganizationId;
            var suppliers = await _unitOfWork.Suppliers.FindAllAsync(s => s.OrganizationId == orgId);
            return _mapper.Map<List<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto?> GetByIdAsync(Guid id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            return supplier is null ? null : _mapper.Map<SupplierDto>(supplier);
        }

        public async Task AddAsync(SupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            supplier.OrganizationId = _sessionService.OrganizationId;

            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
            var existing = await _unitOfWork.Suppliers.GetByIdAsync(supplierDto.Id);
            if (existing is null) return;

            _mapper.Map(supplierDto, existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (existing is null) return;

            _unitOfWork.Suppliers.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
