using FinanceHelper.Domain.Models;

namespace FinanceHelper.Application.Interfaces;

public interface IEmeddingRepository
{
  Task SaveAsync(LabeledVector embedding);
  Task<List<LabeledVector>> LoadAsync();
}