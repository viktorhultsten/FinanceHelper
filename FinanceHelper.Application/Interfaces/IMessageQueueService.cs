namespace FinanceHelper.Application.Interfaces;

public interface IMessageQueueService
{
  Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
  IAsyncEnumerable<T> ConsumeAsync<T>(CancellationToken cancellationToken = default);
}