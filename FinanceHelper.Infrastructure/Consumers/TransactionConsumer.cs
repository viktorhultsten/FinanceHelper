using FinanceHelper.Application.Interfaces;
using FinanceHelper.Core.Messages;
using MassTransit;

namespace FinanceHelper.Infrastructure.Consumers;

public class TransactionConsumer(
  ITransactionRepository _transactionRepository,
  ICategorizerService _categorizerService
) : IConsumer<TransactionMessage>
{
  public async Task Consume(ConsumeContext<TransactionMessage> context)
  {
    var transaction = await _transactionRepository.GetAndHoldByTransactionIdAsync(context.Message.TransactionId);
    if (transaction == null)
    {
      return;
    }

    var category = await _categorizerService.CategorizeAsync(transaction);
    transaction.Category = category;
    await _transactionRepository.SetAndCompleteTransactionAsync(transaction);
  }
}