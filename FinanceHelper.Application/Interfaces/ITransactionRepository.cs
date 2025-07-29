using FinanceHelper.Domain.Models;

namespace FinanceHelper.Application.Interfaces;

public interface ITransactionRepository
{
  public Task AddTransactionsAsync(IEnumerable<TransactionRecord> transactions);
  
  public Task<List<TransactionRecord>> GetAllAsync();
}