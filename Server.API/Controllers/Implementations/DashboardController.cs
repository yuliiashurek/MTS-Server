using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers.Controllers
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

        /// <summary>
        /// Отримання поточних залишків матеріалів по організації.
        /// </summary>
        [ProducesResponseType(typeof(List<MaterialCurrentStockDto>), StatusCodes.Status200OK)]
        [HttpGet("current-stocks")]
        public async Task<ActionResult<List<MaterialCurrentStockDto>>> GetCurrentStocks()
        {
            var organizationId = _session.OrganizationId; 
            var stocks = await _dashboardService.GetCurrentStocksAsync(organizationId);
            return Ok(stocks);
        }

        /// <summary>
        /// Отримання історії залишків обраного матеріалу.
        /// </summary>
        /// <param name="materialId">Ідентифікатор матеріалу.</param>
        [ProducesResponseType(typeof(List<StockHistoryPointDto>), StatusCodes.Status200OK)]
        [HttpGet("{materialId}/stock-history")]
        public async Task<ActionResult<List<StockHistoryPointDto>>> GetStockHistory(Guid materialId)
        {
            var organizationId = _session.OrganizationId;
            var history = await _dashboardService.GetStockHistoryAsync(materialId, organizationId);
            return Ok(history);
        }

        /// <summary>
        /// Отримання прогнозу витрат матеріалів.
        /// </summary>
        [ProducesResponseType(typeof(List<ForecastDashboardItemDto>), StatusCodes.Status200OK)]
        [HttpGet("forecast")]
        public async Task<ActionResult<List<ForecastDashboardItemDto>>> GetForecastDashboard()
        {
            var result = await _dashboardService.GetForecastDashboardItemsAsync(_session.OrganizationId);
            return Ok(result);
        }

        /// <summary>
        /// Отримання історії витрат обраного матеріалу.
        /// </summary>
        /// <param name="materialId">Ідентифікатор матеріалу.</param>
        [ProducesResponseType(typeof(List<MaterialOutflowPointDto>), StatusCodes.Status200OK)]
        [HttpGet("forecast/{materialId}/outflow")]
        public async Task<ActionResult<List<MaterialOutflowPointDto>>> GetMaterialOutflowHistory(Guid materialId)
        {
            var organizationId = _session.OrganizationId;
            var history = await _dashboardService.GetMaterialOutflowHistoryAsync(materialId, organizationId);
            return Ok(history);
        }

    }
}
