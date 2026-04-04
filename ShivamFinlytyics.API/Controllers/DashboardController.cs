using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Application.Services;

namespace ShivamFinlytyics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("summary")] // Add this attribute
        public async Task<IActionResult> Summary()
        {
            var data = await _service.GetSummary();
            return Ok(data);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpGet("category")]
        public async Task<IActionResult> Category()
        {
            var data = await _service.GetCategoryBreakDown();
            return Ok(data);
        }



    }
}
