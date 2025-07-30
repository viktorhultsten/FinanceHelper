using FinanceHelper.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Infrastructure.Data;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options) : DbContext(options)
{
  public DbSet<TransactionRecord> Transactions => Set<TransactionRecord>();

  public DbSet<LabeledVector> Embeddings => Set<LabeledVector>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<TransactionRecord>()
        .Property(e => e.Date)
        .HasColumnType("date");

    builder.HasPostgresExtension("vector");
    builder.Entity<LabeledVector>()
      .Property(e => e.Embedding)
      .HasColumnType("vector(1536)");
  }
}
