using System;
using ShivamFinlytics.Application.DTOs;

namespace ShivamFinlytics.Application.Interfaces;

public interface IActivityLogService
{
    Task Log(int userId,string action,string entity,int entityId,string details);

    Task<IEnumerable<ActivityLogDto>> GetAllLogsAsync();
    Task<IEnumerable<ActivityLogDto>> GetLogsByUserIdAsync(int userId);

}
