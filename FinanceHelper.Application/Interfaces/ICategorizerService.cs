using FinanceHelper.Domain.Models;

namespace FinanceHelper.Application.Interfaces;

public interface ICategorizerService
{
  public Task<string> CategorizeAsync(TransactionRecord transaction);
}