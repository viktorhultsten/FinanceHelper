namespace FinanceHelper.Application.Interfaces;

public interface IMessageQueueService
{
  Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
  Task ConsumeAsync(Func<string, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default);
}