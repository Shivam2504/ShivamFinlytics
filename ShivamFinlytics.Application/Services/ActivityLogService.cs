using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Infrastructure.Data;

namespace ShivamFinlytics.Application.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly AppDbContext _context;
    public ActivityLogService(AppDbContext context)
    {
        _context = context;
    }
    public async Task Log(int userId, string action, string entity, int entityId, string details)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            EntityName = entity,
            EntityId = entityId,
            Details = details
        };
        _context.ActivityLogs.Add(log);
        await _context.SaveChangesAsync();
        
    }
    public async Task<IEnumerable<ActivityLogDto>> GetAllLogsAsync()
{
    return await _context.ActivityLogs
        .Include(l => l.User)
        .OrderByDescending(l => l.TimeStamp)
        .Select(l => new ActivityLogDto
        {
            ActivityId = l.ActivityId,
            UserName = l.User.Name, // Map User.Name to UserName
            Action = l.Action,
            EntityName = l.EntityName,
            EntityId = l.EntityId,
            TimeStamp = l.TimeStamp,
            Details = l.Details
        })
        .ToListAsync();
}

public async Task<IEnumerable<ActivityLogDto>> GetLogsByUserIdAsync(int userId)
{
    return await _context.ActivityLogs
        .Where(l => l.UserId == userId)
        .OrderByDescending(l => l.TimeStamp)
        .Select(l => new ActivityLogDto
        {
            ActivityId = l.ActivityId,
            UserName = l.User.Name,
            Action = l.Action,
            EntityName = l.EntityName,
            EntityId = l.EntityId,
            TimeStamp = l.TimeStamp,
            Details = l.Details
        })
        .ToListAsync();
}
}
