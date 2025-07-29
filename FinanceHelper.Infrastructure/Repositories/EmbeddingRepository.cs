using System.Text.Json;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using FinanceHelper.Domain.Models;
using Microsoft.Extensions.Options;

namespace FinanceHelper.Infrastructure.Repositories;

public class EmbeddingRepository (IOptions<EmbeddingServiceOptions> _options) : IEmeddingRepository
{

  private readonly string filePath = _options.Value.FilePath;
  public async Task<List<LabeledVector>> LoadAsync()
  {
    if (!File.Exists(filePath)) return [];

    var json = await File.ReadAllTextAsync(filePath);
    return JsonSerializer.Deserialize<List<LabeledVector>>(json) ?? [];
  }

  public async Task SaveAsync(List<LabeledVector> data)
  {
    var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
    {
      WriteIndented = true
    });

    await File.WriteAllTextAsync(filePath, json);
  }
}