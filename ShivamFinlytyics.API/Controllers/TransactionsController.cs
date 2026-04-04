using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionservice;

        public TransactionsController(ITransactionsService transactionservice)
        {
            _transactionservice = transactionservice;
        }

        // 🆕 ADD THIS: The missing GET method for Swagger
        [HttpGet("all")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionservice.GetAll();
            
            if (transactions == null || !transactions.Any())
            {
                return NotFound(new { message = "No transactions found." });
            }

            return Ok(transactions);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token");

            int userid = int.Parse(userIdClaim.Value);
            await _transactionservice.CreateTrancsaction(dto, userid);
            
            return Ok(new { message = "Transaction created successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try 
            {
                await _transactionservice.DeleteTransaction(id);
                return Ok(new { message = "Transaction deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}