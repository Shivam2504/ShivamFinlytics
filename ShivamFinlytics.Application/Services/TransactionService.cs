using System;
using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Infrastructure.Data;
using ShivamFinlytics.Domain.Entities;
using System.Xml;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Application.Services;

public class TransactionService : ITransactionsService
{
    private readonly AppDbContext _context;
    private readonly IActivityLogService _logService;
    public TransactionService(AppDbContext context, IActivityLogService logService)
    {
        _context = context;
        _logService = logService;
    }
    public async Task CreateTrancsaction(CreateTransactionDto dto, int userId)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            Amount = dto.Amount,
            Type = dto.Type,
            CategoryId = dto.CategoryId,
            Note = dto.Note,
            Date = dto.Date == default ? DateTime.UtcNow : dto.Date

        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

    }

    public async Task DeleteTransaction(int id)
    {
        var trans = await _context.Transactions.FindAsync(id);
        if (trans == null)
        {
            throw new Exception("Transaction with this transaction id is not found");
        }

        trans.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<TransactionDto>> GetAll()
    {
        return await _context.Transactions
            .Include(t => t.Category) 
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                Type = t.Type.ToString(), 
                CategoryName = t.Category.Name,
                Note = t.Note,
                Date = t.Date
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TransactionFilterDto>> GetTransactionsByTypeAsync(TransactionType type)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(t => t.Type == type) // Direct enum comparison
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionFilterDto
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                // Convert Enum to String for the JSON response
                Type = t.Type.ToString(),
                CategoryName = t.Category.Name,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TransactionFilterDto>> GetTransactionsByDateAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Transactions
            .AsNoTracking()
            // .Date strips the time from the DB column for a clean comparison
            .Where(t => t.CreatedAt.Date >= startDate.Date && t.CreatedAt.Date <= endDate.Date)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionFilterDto
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                Type = t.Type.ToString(),
                CategoryName = t.Category.Name,
                Date = t.Date,       // User's recorded date
                CreatedAt = t.CreatedAt // Actual system timestamp
            })
            .ToListAsync();
    }
}
