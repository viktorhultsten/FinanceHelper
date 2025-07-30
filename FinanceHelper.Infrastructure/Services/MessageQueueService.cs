using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinanceHelper.Infrastructure.Services;

public class MessageQueueService(
  IOptions<QueueOptions> _options
) : IMessageQueueService, IDisposable
{
  private IConnection? _connection;
  private IChannel? _channel;
  private readonly string _queueName = "transactions";

  public async Task ConsumeAsync(Func<string, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default)
  {
    await EnsureConnectionAsync(cancellationToken);
    var consumer = new AsyncEventingBasicConsumer(_channel!);
    consumer.ReceivedAsync += (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      onMessage(message, cancellationToken);
      return Task.CompletedTask;
    };
    await _channel!.BasicConsumeAsync(_queueName, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
  }

  public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
  {
    await EnsureConnectionAsync(cancellationToken);
    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
    var props = new BasicProperties
    {
      DeliveryMode = DeliveryModes.Persistent
    };
    await _channel!.BasicPublishAsync(
      string.Empty,
      routingKey: _queueName,
      mandatory: true,
      basicProperties: props,
      body: body,
      cancellationToken: cancellationToken
    );
  }

  private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
  {
    if (_connection != null && _channel != null)
    {
      return;
    }

    var factory = new ConnectionFactory()
    {
      HostName = _options.Value.HostName
    };
    _connection = await factory.CreateConnectionAsync(cancellationToken);
    _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
    await _channel.QueueDeclareAsync(
      queue: "transactions",
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null,
      cancellationToken: cancellationToken
    );
  }

  public void Dispose()
  {
    _channel?.Dispose();
    _connection?.Dispose();
  }
}