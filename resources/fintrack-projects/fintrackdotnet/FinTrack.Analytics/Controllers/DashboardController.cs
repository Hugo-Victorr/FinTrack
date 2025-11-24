using System.Security.Claims;
using FinTrack.Analytics.Contracts;
using FinTrack.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Analytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [HttpGet]
        [ProducesResponseType(typeof(DashboardSummaryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummaryAsync([FromQuery] string period = "current")
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return BadRequest(new { error = "User ID not found in claims" });
                }

                var summary = await _dashboardService.GetDashboardSummaryAsync(userId, period);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching dashboard data", message = ex.Message });
            }
        }
    }
}


