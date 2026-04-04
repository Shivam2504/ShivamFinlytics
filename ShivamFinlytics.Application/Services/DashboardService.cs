using System;
using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Infrastructure.Data;

namespace ShivamFinlytics.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<object> GetCategoryBreakDown()
    {
        return await _context.Transactions
                        .Where(t => !t.IsDeleted)
                        .GroupBy(t => t.Category.Name)
                        .Select(g => new
                        {
                            Category = g.Key,
                            Total = g.Sum(x => x.Amount)
                        }).ToListAsync();
       
    }

    public async Task<object> GetSummary()
    {
        var income = await _context.Transactions.Where(t => t.Type == Domain.Enums.TransactionType.Income && !t.IsDeleted).SumAsync(t => t.Amount);
        var expance = await _context.Transactions.Where(t => t.Type == Domain.Enums.TransactionType.Exapance && !t.IsDeleted).SumAsync(t => t.Amount);
        return new
        {
            TotalIncome = income,
            TotalExpance = expance,
            NetBalance = income - expance
        };
        
    }
}
