using ExpensesTracker.Models.Dashboard;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardSummaryController : Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardSummaryController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary(
            [FromQuery] DashboardFilterDto dashboardFilterDto)
        {
            var result = await this.dashboardService.GetSummaryAsync(dashboardFilterDto);

            return Ok(result);
        }
    }
}
