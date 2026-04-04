using System;

namespace ShivamFinlytics.Application.DTOs;

public class TransactionFilterDto
{
    public int TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string? Type { get; set; } // e.g., "Income" or "Expense"
    public string? CategoryName { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt {get;set;}

}
