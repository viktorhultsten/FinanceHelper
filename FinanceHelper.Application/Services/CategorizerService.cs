using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Common;
using FinanceHelper.Domain.Models;
using Pgvector;

namespace FinanceHelper.Application.Services;

public class CategorizerService(
  IEmbeddingService _embeddingService,
  IEmeddingRepository _embeddingRepository
) : ICategorizerService
{
  public async Task<string> CategorizeAsync(TransactionRecord transaction)
  {
    var examples = await _embeddingRepository.LoadAsync();

    var prompt = $"{transaction.Description} || {transaction.Date:yyyy-MM-dd} || {transaction.Amount}";
    var exactExample = examples.FirstOrDefault(example => example.Text.Equals(prompt));

    if (exactExample != null)
    {
      return exactExample.Category;
    }

    var embedding = await _embeddingService.EmbedTextAsync(prompt);
    var bestMatch = examples
            .Where(ex => ex.Embedding != null)
            .Select(example => new
            {
              example.Category,
              Similarity = VectorUtils.CosineSimilarity(embedding, example.Embedding!.ToArray())
            })
            .OrderByDescending(x => x.Similarity)
            .FirstOrDefault();

    var bestCategory = bestMatch != null && bestMatch.Similarity > 0.8f ? bestMatch.Category : "Uncategorized";

    if (!examples.Any(example => example.Text == transaction.Description))
    {
      var newExample = new LabeledVector()
      {
        Category = bestCategory,
        Text = prompt,
        Embedding = new Vector(embedding),
      };

      // await _embeddingRepository.SaveAsync(newExample);
    }

    return bestCategory;
  }
}
