using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using FinanceHelper.Application.Services;
using FinanceHelper.Infrastructure.Data;
using FinanceHelper.Infrastructure.Repositories;
using FinanceHelper.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EmbeddingServiceOptions>(
    builder.Configuration.GetSection("EmbeddingService"));

builder.Services.Configure<QueueOptions>(
    builder.Configuration.GetSection("QueueOptions"));

builder.Services.AddScoped<IEmeddingRepository, EmbeddingRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<ICategorizerService, CategorizerService>();

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
