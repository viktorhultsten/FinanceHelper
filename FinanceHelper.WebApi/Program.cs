using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using FinanceHelper.Application.Services;
using FinanceHelper.Infrastructure.Consumers;
using FinanceHelper.Infrastructure.Data;
using FinanceHelper.Infrastructure.Repositories;
using FinanceHelper.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOpts => npgsqlOpts.UseVector()));



builder.Services.Configure<EmbeddingServiceOptions>(
    builder.Configuration.GetSection("EmbeddingService"));

builder.Services.Configure<QueueOptions>(
    builder.Configuration.GetSection("QueueOptions"));

builder.Services.AddScoped<IEmeddingRepository, EmbeddingRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<ICategorizerService, CategorizerService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TransactionConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var opts = context.GetRequiredService<IOptions<QueueOptions>>().Value;
        cfg.Host(new Uri(opts.ConnectionString));
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMinioClient(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
