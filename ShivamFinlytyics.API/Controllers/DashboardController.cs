using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers;

/// <summary>
/// Provides aggregated financial data and high-level summaries for the dashboard.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a general financial summary including totals and recent activity.
    /// </summary>
    /// <returns>A summary object containing key financial metrics.</returns>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Summary()
    {
        var data = await _service.GetSummary();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves a detailed breakdown of finances grouped by category.
    /// Access restricted to Administrators.
    /// </summary>
    /// <returns>A list of financial data partitioned by category.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Category()
    {
        var data = await _service.GetCategoryBreakDown();
        return Ok(data);
    }
}