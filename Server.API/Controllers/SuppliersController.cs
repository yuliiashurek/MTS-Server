using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Server.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var suppliers = await _supplierService.GetAllAsync();
            var dtoList = _mapper.Map<List<SupplierDto>>(suppliers);
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupplierDto dto)
        {
            await _supplierService.AddAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SupplierDto dto)
        {
            if (id != dto.Id) 
                return BadRequest();
            await _supplierService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _supplierService.DeleteAsync(id);
            return NoContent();
        }
    }
}
