using System;
using System.Transactions;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Application.DTOs;

public class CreateTransactionDto
{
    public decimal Amount {get;set;}
    public int CategoryId {get;set;}

    public TransactionType Type {get;set;}

    public DateTime Date {get;set;}

    public required string Note{get;set;}

}
