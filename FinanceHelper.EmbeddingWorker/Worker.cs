using FinanceHelper.Application.Interfaces;

namespace FinanceHelper.EmbeddingWorker;

public class Worker(
    ILogger<Worker> logger,
    IMessageQueueService _messageQueueService
) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageQueueService.ConsumeAsync(OnMessageAsync, stoppingToken);
    }

    private Task OnMessageAsync(string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"received msg: {message}");
        return Task.CompletedTask;
    }
}
