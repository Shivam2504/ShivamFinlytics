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

        var insight = new AnalystInsight
        {
            Title = dto.Title,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            TranctionId = dto.TransactionId,
            ImpactLevel = Enum.Parse<ImpactLevel>(dto.ImpactLevel, true),
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
