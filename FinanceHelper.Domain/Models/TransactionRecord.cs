using FinanceHelper.Domain.Enums;

namespace FinanceHelper.Domain.Models;

public class TransactionRecord
{
  public int Id { get; set; }
  public DateTime Date { get; set; }
  public string Description { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public string Category { get; set; } = "Uncategorized";
  public ProcessingState ProcessingState { get; set; } = ProcessingState.notStarted;
}