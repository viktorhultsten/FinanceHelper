namespace FinanceHelper.Application.Interfaces;

public interface IEmbeddingService
{
  public Task<float[]> EmbedTextAsync(string input);
}