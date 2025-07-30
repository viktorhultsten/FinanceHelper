using FinanceHelper.Domain.Models;

namespace FinanceHelper.Application.Interfaces;

public interface ITransactionRepository
{
  public Task AddTransactionsAsync(IEnumerable<TransactionRecord> transactions);

  public Task<List<TransactionRecord>> GetAllAsync();
  public Task<TransactionRecord?> GetAndHoldByTransactionIdAsync(int transactionId);
  public Task<string?> GetCategoryByDescriptionAsync(string description);
  public Task SetAndCompleteTransactionAsync(TransactionRecord transaction);
}