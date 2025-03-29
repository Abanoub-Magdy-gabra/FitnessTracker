using System.Threading.Tasks;
using FitnessTracker.API.Models.Responses.Dashboard;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryResponse>> GetDashboardSummary()
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(summary);
        }

        [HttpGet("user-stats")]
        public async Task<ActionResult<UserStatsResponse>> GetUserStats([FromQuery] string period = "month")
        {
            var stats = await _dashboardService.GetUserStatsAsync(period);
            return Ok(stats);
        }

        [HttpGet("feature-usage")]
        public async Task<ActionResult<FeatureUsageResponse>> GetFeatureUsage()
        {
            var usage = await _dashboardService.GetFeatureUsageAsync();
            return Ok(usage);
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<RevenueResponse>> GetRevenue([FromQuery] string period = "month")
        {
            var revenue = await _dashboardService.GetRevenueAsync(period);
            return Ok(revenue);
        }

        [HttpGet("recent-activities")]
        public async Task<ActionResult<RecentActivitiesResponse>> GetRecentActivities()
        {
            var activities = await _dashboardService.GetRecentActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("recent-users")]
        public async Task<ActionResult<RecentUsersResponse>> GetRecentUsers()
        {
            var users = await _dashboardService.GetRecentUsersAsync();
            return Ok(users);
        }

        [HttpGet("system-alerts")]
        public async Task<ActionResult<SystemAlertsResponse>> GetSystemAlerts()
        {
            var alerts = await _dashboardService.GetSystemAlertsAsync();
            return Ok(alerts);
        }
    }
}