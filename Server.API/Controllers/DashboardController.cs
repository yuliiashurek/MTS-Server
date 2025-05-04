using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        private readonly ISessionService _session;

        public DashboardController(IDashboardService dashboardService, ISessionService session)
        {
            _session = session;
            _dashboardService = dashboardService;
        }

        [HttpGet("current-stocks")]
        public async Task<ActionResult<List<MaterialCurrentStockDto>>> GetCurrentStocks()
        {
            var organizationId = _session.OrganizationId; 
            var stocks = await _dashboardService.GetCurrentStocksAsync(organizationId);
            return Ok(stocks);
        }

        [HttpGet("{materialId}/stock-history")]
        public async Task<ActionResult<List<StockHistoryPointDto>>> GetStockHistory(Guid materialId)
        {
            var organizationId = _session.OrganizationId;
            var history = await _dashboardService.GetStockHistoryAsync(materialId, organizationId);
            return Ok(history);
        }

        [HttpGet("forecast")]
        public async Task<ActionResult<List<ForecastDashboardItemDto>>> GetForecastDashboard()
        {
            var result = await _dashboardService.GetForecastDashboardItemsAsync(_session.OrganizationId);
            return Ok(result);
        }

        [HttpGet("forecast/{materialId}/outflow")]
        public async Task<ActionResult<List<MaterialOutflowPointDto>>> GetMaterialOutflowHistory(Guid materialId)
        {
            var organizationId = _session.OrganizationId;
            var history = await _dashboardService.GetMaterialOutflowHistoryAsync(materialId, organizationId);
            return Ok(history);
        }


    }
}
