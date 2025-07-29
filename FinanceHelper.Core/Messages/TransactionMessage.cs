namespace FinanceHelper.Core.Messages;

public record TransactionMessage
{
  public int TransactionId { get; init; }
}