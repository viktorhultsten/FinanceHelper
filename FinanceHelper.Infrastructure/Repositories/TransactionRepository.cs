using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Models;
using FinanceHelper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Infrastructure.Repositories;

public class TransactionRepository (
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
}