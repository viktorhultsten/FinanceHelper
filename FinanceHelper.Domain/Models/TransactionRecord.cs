using System.ComponentModel.DataAnnotations;
using FinanceHelper.Domain.Enums;
using Pgvector;

namespace FinanceHelper.Domain.Models;

public class TransactionRecord
{
  [Key]
  public int Id { get; set; }
  public DateTime Date { get; set; }
  public string Description { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public string Category { get; set; } = "Uncategorized";
  public ProcessingState ProcessingState { get; set; } = ProcessingState.notStarted;
  public bool UserApproved { get; set; } = false;
}