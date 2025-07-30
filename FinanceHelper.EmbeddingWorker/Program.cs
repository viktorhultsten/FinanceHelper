using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using FinanceHelper.EmbeddingWorker;
using FinanceHelper.Infrastructure.Data;
using FinanceHelper.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<QueueOptions>(
    builder.Configuration.GetSection("QueueOptions"));

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IMessageQueueService, MessageQueueService>();

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var host = builder.Build();
host.Run();
