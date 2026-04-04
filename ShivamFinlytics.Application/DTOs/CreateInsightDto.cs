using System;

namespace ShivamFinlytics.Application.DTOs;

public class CreateInsightDto
{
    public required string Title {get;set;}

    public required string Description{get;set;}

    public int? CategoryId {get;set;}

    public int? TransactionId {get;set;}

    public required string ImpactLevel {get;set;}

}
