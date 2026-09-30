using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardSummaryController : Controller
    {
        private readonly IMapper mapper;
        private readonly IDashboardService dashboardService;

        public DashboardSummaryController(
            IMapper mapper,
            IDashboardService dashboardService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        }

        [HttpGet]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary(
            [FromQuery] DashboardFilterDto dashboardFilterDto)
        {
            var dashboardQuery = this.mapper.Map<DashboardQuery>(dashboardFilterDto);
            var result = await this.dashboardService.GetSummaryAsync(dashboardQuery);

            return Ok(result);
        }
    }
}
