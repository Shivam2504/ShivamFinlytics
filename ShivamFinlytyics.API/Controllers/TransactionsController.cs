using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers;

/// <summary>
/// Manages financial transactions, including retrieval, creation, and deletion.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionsService _transactionservice;

    public TransactionsController(ITransactionsService transactionservice)
    {
        _transactionservice = transactionservice;
    }

    /// <summary>
    /// Retrieves all recorded transactions across the system.
    /// Restricted to Users with the 'Admin' role.
    /// </summary>
    /// <returns>A list of all transactions.</returns>
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _transactionservice.GetAll();

        if (transactions == null || !transactions.Any())
        {
            return NotFound(new { message = "No transactions found." });
        }

        return Ok(transactions);
    }

    /// <summary>
    /// Creates a new financial transaction associated with the authenticated user.
    /// Restricted to Users with the 'Admin' role.
    /// </summary>
    /// <param name="dto">The transaction details.</param>
    /// <returns>A success message upon creation.</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
        
        if (userIdClaim == null) 
        {
            return Unauthorized(new { message = "User ID not found in token." });
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return BadRequest(new { message = "Invalid User ID format in token." });
        }

        await _transactionservice.CreateTrancsaction(dto, userId);
        return Ok(new { message = "Transaction created successfully" });
    }

    /// <summary>
    /// Deletes a specific transaction by its unique identifier.
    /// Restricted to Users with the 'Admin' role.
    /// </summary>
    /// <param name="id">The ID of the transaction to delete.</param>
    /// <returns>A success message or a not found error.</returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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