using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Enums;

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
    /// <summary>
    /// Filters transactions based on a start and end date.
    /// </summary>
    /// <param name="startDate">The beginning of the period (YYYY-MM-DD).</param>
    /// <param name="endDate">The end of the period (YYYY-MM-DD).</param>
    [HttpGet("filter")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(new { message = "Start date cannot be after the end date." });
        }

        var data = await _transactionservice.GetTransactionsByDateAsync(startDate, endDate);
        return Ok(data);
    }

    /// <summary>
    /// Filters transactions by Type using an Enum.
    /// </summary>
    /// <param name="type">Choose 1 for Income, 2 for Expense</param>
    [HttpGet("filter-by-type")]
    public async Task<IActionResult> GetByType([FromQuery] TransactionType type)
    {
        // .NET automatically validates if the value is part of the Enum
        var results = await _transactionservice.GetTransactionsByTypeAsync(type);
        return Ok(results);
    }
}