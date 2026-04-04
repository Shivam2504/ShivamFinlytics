using System;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Domain.Entities;

namespace ShivamFinlytics.Application.Interfaces;

public interface IInsightService
{
    Task CreateInsight(CreateInsightDto dto,int userId);
    Task<List<AnalystInsight>> GetAll();

}
