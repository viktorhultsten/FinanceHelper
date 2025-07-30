using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Enums;
using FinanceHelper.Domain.Models;
using FinanceHelper.Infrastructure.Data;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Infrastructure.Repositories;

public class TransactionRepository(
  FinanceDbContext _context
) : ITransactionRepository
{
  public async Task AddTransactionsAsync(IEnumerable<TransactionRecord> transactions)
  {
    await _context.Transactions.AddRangeAsync(transactions);
    await _context.SaveChangesAsync();
  }

  public async Task<List<TransactionRecord>> GetAllAsync()
  {
    return await _context.Transactions.ToListAsync();
  }

  public async Task<TransactionRecord?> GetAndHoldByTransactionIdAsync(int transactionId)
  {
    var updatedCount = await _context.Transactions
      .Where(t => t.Id == transactionId && t.ProcessingState == ProcessingState.notStarted)
      .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.ProcessingState, ProcessingState.processing));

    if (updatedCount == 0)
    {
      return null;
    }
    return await _context.Transactions.SingleOrDefaultAsync(t => t.Id == transactionId);
  }

  public async Task<string?> GetCategoryByDescriptionAsync(string description)
  {
    return await _context.Transactions
        .Where(t =>
            t.Description == description
         && t.Category != "Uncategorized")
        .Select(t => t.Category)
        .FirstOrDefaultAsync();
  }

  public async Task SetAndCompleteTransactionAsync(TransactionRecord transaction)
  {
    transaction.ProcessingState = ProcessingState.complete;
    _context.Transactions.Update(transaction);
    await _context.SaveChangesAsync();
  }
}