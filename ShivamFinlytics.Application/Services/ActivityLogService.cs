using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Infrastructure.Data;

namespace ShivamFinlytics.Application.Services;

/// <summary>
/// Provides logging and retrieval services for user activities and system audits.
/// </summary>
public class ActivityLogService : IActivityLogService
{
    private readonly AppDbContext _context;

    public ActivityLogService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Records a new activity log entry in the database.
    /// </summary>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <param name="action">The description of the action (e.g., "Create", "Update").</param>
    /// <param name="entity">The name of the entity being affected.</param>
    /// <param name="entityId">The unique identifier of the affected entity.</param>
    /// <param name="details">Additional metadata or descriptions of the change.</param>
    public async Task Log(int userId, string action, string entity, int entityId, string details)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            EntityName = entity,
            EntityId = entityId,
            Details = details,
            TimeStamp = DateTime.UtcNow // Ensuring UTC consistency
        };

        _context.ActivityLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all system activity logs, ordered by the most recent first.
    /// </summary>
    /// <returns>A collection of <see cref="ActivityLogDto"/> containing audit details.</returns>
    public async Task<IEnumerable<ActivityLogDto>> GetAllLogsAsync()
    {
        return await _context.ActivityLogs
            .AsNoTracking()
            .Include(l => l.User)
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

    /// <summary>
    /// Retrieves all activity logs associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A filtered collection of activity logs for the specified user.</returns>
    public async Task<IEnumerable<ActivityLogDto>> GetLogsByUserIdAsync(int userId)
    {
        return await _context.ActivityLogs
            .AsNoTracking()
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