using System;
using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Infrastructure.Data;
using ShivamFinlytics.Domain.Entities;
using System.Xml;

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
            Note = dto.Note
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
            .Include(t => t.Category) // Join with Category table
            .Where(t => !t.IsDeleted) // 🛡️ Soft Delete filter
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                Type = t.Type.ToString(), // Converts Enum to String
                CategoryName = t.Category.Name,
                Note = t.Note,
                Date = t.Date
            })
            .ToListAsync();
    }
}
