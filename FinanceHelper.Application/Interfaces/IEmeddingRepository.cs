using FinanceHelper.Domain.Models;

namespace FinanceHelper.Application.Interfaces;

public interface IEmeddingRepository
{
  Task SaveAsync(List<LabeledVector> data);
  Task<List<LabeledVector>> LoadAsync();
}