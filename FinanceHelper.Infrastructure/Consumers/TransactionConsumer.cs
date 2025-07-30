using FinanceHelper.Core.Messages;
using MassTransit;

namespace FinanceHelper.Infrastructure.Consumers;

public class TransactionConsumer : IConsumer<TransactionMessage>
{
  public Task Consume(ConsumeContext<TransactionMessage> context)
  {
    Console.WriteLine($"Received: {context.Message.TransactionId}");
    return Task.CompletedTask;
  }
}