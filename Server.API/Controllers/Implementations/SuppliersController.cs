using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Server.API.Controllers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        /// <summary>
        /// Отримати список усіх постачальників.
        /// </summary>
        [ProducesResponseType(typeof(List<SupplierDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var suppliers = await _supplierService.GetAllAsync();
            var dtoList = _mapper.Map<List<SupplierDto>>(suppliers);
            return Ok(dtoList);
        }

        /// <summary>
        /// Додати нового постачальника.
        /// </summary>
        /// <param name="dto">Дані постачальника.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupplierDto dto)
        {
            await _supplierService.AddAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Оновити дані постачальника.
        /// </summary>
        /// <param name="id">ID постачальника.</param>
        /// <param name="dto">Оновлені дані постачальника.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SupplierDto dto)
        {
            if (id != dto.Id) 
                return BadRequest();
            await _supplierService.UpdateAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// Видалити постачальника за ID.
        /// </summary>
        /// <param name="id">ID постачальника.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _supplierService.DeleteAsync(id);
            return NoContent();
        }
    }
}
