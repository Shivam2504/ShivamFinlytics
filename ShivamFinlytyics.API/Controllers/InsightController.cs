using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Application.Services;

namespace ShivamFinlytyics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsightController : ControllerBase
    {
        private readonly IInsightService _service;
        public InsightController(IInsightService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Analyst,Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateInsightDto dto)
        {
            // Try both standard and custom ID claim names
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid User ID in token.");
            }

            await _service.CreateInsight(dto, userId);
            return Ok("Insight created successfully");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }
    }
}
