using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Data.Entities;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _supplierService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Supplier supplier)
        {
            await _supplierService.AddAsync(supplier);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Supplier supplier)
        {
            if (id != supplier.Id) return BadRequest();
            await _supplierService.UpdateAsync(supplier);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _supplierService.DeleteAsync(id);
            return NoContent();
        }
    }

}
