using FinanceHelper.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceHelper.Infrastructure.Data;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options) : DbContext(options)
{
  public DbSet<TransactionRecord> Transactions => Set<TransactionRecord>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<TransactionRecord>()
        .Property(e => e.Date)
        .HasColumnType("date");
  }
}
