using System;

namespace ShivamFinlytics.Application.Interfaces;

public interface IDashboardService
{
    Task<object> GetSummary();
    Task<object> GetCategoryBreakDown();

}
