using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Common;
using FinanceHelper.Domain.Models;

namespace FinanceHelper.Application.Services;

public class CategorizerService(
  IEmbeddingService _embeddingService,
  IEmeddingRepository _embeddingRepository
) : ICategorizerService
{
  public async Task<string> CategorizeAsync(string text)
  {
    var examples = await _embeddingRepository.LoadAsync();

    var exactExample = examples.FirstOrDefault(example => example.Text.Equals(text));

    if (exactExample != null)
    {
      return exactExample.Category;
    }

    var embedding = await _embeddingService.EmbedTextAsync(text);
    var bestMatch = examples
            .Select(example => new
            {
              example.Category,
              Similarity = VectorUtils.CosineSimilarity(embedding, example.Embedding)
            })
            .OrderByDescending(x => x.Similarity)
            .FirstOrDefault();

    var bestCategory = bestMatch != null && bestMatch.Similarity > 0.85f ? bestMatch.Category : "Other";

    Console.WriteLine($"\"{text}\" got {bestMatch?.Category} with {bestMatch?.Similarity} similarity");

    if (!examples.Any(example => example.Text == text))
    {
      var newExample = new LabeledVector()
      {
        Category = bestCategory,
        Text = text,
        Embedding = embedding,
      };

      await _embeddingRepository.SaveAsync([.. examples, newExample]);
    }

    return bestCategory;
  }
}
