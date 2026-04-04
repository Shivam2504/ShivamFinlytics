using System;
using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Domain.Enums;
using ShivamFinlytics.Infrastructure.Data;

namespace ShivamFinlytics.Application.Services;

public class InsightService : IInsightService
{
    private readonly AppDbContext _context;
    private readonly IActivityLogService _logger;
    public InsightService(AppDbContext context,IActivityLogService logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task CreateInsight(CreateInsightDto dto, int userId)
    {
        // // 1. Check if User exists
        // var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
        // if (!userExists)
        // {
        //     throw new Exception($"Unauthorized: User ID {userId} does not exist in the database. Please log in again.");
        // }

        // // 2. Check if Transaction exists (from the previous error)
        // var transactionExists = await _context.Transactions.AnyAsync(t => t.TransactionId == dto.TransactionId);
        // if (!transactionExists)
        // {
        //     throw new Exception($"Transaction ID {dto.TransactionId} not found.");
        // }

        var insight = new AnalystInsight
        {
            Title = dto.Title,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            // ✅ Ensure this matches the typo in your Entity: "TranctionId"
            TranctionId = dto.TransactionId,
            ImpactLevel = Enum.Parse<ImpactLevel>(dto.ImpactLevel, true),

            // 🚀 CRITICAL FIX: Map the userId to CreatedBy
            CreatedBy = userId,

            CreatedAt = DateTime.UtcNow
        };

        _context.AnalystInsights.Add(insight);
        await _context.SaveChangesAsync();

        await _logger.Log(
            userId, 
            "CREATE", 
            "AnalystInsight", 
            insight.Id, 
            $"Insight '{insight.Title}' created by user {userId}"
        );
    }

    public async Task<List<AnalystInsight>> GetAll()
    {
        return await _context.AnalystInsights
                                .Include(i => i.Category)
                                .Include(i => i.Transaction)
                                .ToListAsync();
    }
}
