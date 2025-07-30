using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Models;
using FinanceHelper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Infrastructure.Repositories;

public class EmbeddingRepository(
  // IOptions<EmbeddingServiceOptions> _options,
  FinanceDbContext _context
  ) : IEmeddingRepository
{


  public async Task<List<LabeledVector>> LoadAsync()
  {
    return await _context.Embeddings.ToListAsync() ?? [];
  }

  public async Task SaveAsync(LabeledVector embedding)
  {
    await _context.Embeddings.AddAsync(embedding);
    await _context.SaveChangesAsync();
  }
}