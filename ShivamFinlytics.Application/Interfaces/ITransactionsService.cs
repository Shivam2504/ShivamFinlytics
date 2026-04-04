using System;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Application.Interfaces;

public interface ITransactionsService
{
    Task CreateTrancsaction(CreateTransactionDto dto, int userId);
    Task<List<TransactionDto>> GetAll();
    Task DeleteTransaction(int id);

    /// <summary>
    /// Retrieves transactions filtered by a specific date range.
    /// </summary>
    Task<IEnumerable<TransactionFilterDto>> GetTransactionsByDateAsync(DateTime startDate, DateTime endDate);

    Task<IEnumerable<TransactionFilterDto>> GetTransactionsByTypeAsync(TransactionType Type);

}
