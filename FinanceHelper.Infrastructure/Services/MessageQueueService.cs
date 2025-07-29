using System.Text;
using System.Text.Json;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FinanceHelper.Infrastructure.Services;

public class MessageQueueService(
  IOptions<QueueOptions> _options
) : IMessageQueueService
{
  public IAsyncEnumerable<T> ConsumeAsync<T>(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
  {
    var factory = new ConnectionFactory()
    {
      HostName = _options.Value.HostName
    };
    using var connection = await factory.CreateConnectionAsync(cancellationToken);
    using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
    var res = await channel.QueueDeclareAsync(
      queue: "transactions",
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null,
      cancellationToken: cancellationToken
    );
    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
    var props = new BasicProperties
    {
      DeliveryMode = DeliveryModes.Persistent
    };
    await channel.BasicPublishAsync(
      "",
      routingKey: "transactions",
      mandatory: true,
      basicProperties: props,
      body: body,
      cancellationToken: cancellationToken
    );
  }
}