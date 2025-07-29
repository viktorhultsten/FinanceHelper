using System.Net.Http.Json;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using Microsoft.Extensions.Options;

namespace FinanceHelper.Infrastructure.Services;

public class EmbeddingService : IEmbeddingService
{
  private readonly HttpClient _http;
  private readonly string _apiKey;

  public EmbeddingService(IOptions<EmbeddingServiceOptions> options)
  {
    _http = new HttpClient();
    _apiKey = options.Value.ApiKey;
    _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
  }

  public async Task<float[]> EmbedTextAsync(string input)
  {
    var content = new
    {
      model = "text-embedding-3-small",
      input = input
    };

    var response = await _http.PostAsJsonAsync("https://api.openai.com/v1/embeddings", content);
    response.EnsureSuccessStatusCode();

    var json = await response.Content.ReadFromJsonAsync<OpenAiEmbeddingResponse>();
    return json?.Data?.FirstOrDefault()?.Embedding ?? throw new Exception("No embedding returned.");
  }

  //TODO Move models
  private class OpenAiEmbeddingResponse
  {
    public List<EmbeddingData> Data { get; set; } = [];
  }

  private class EmbeddingData
  {
    public float[] Embedding { get; set; } = [];
  }
}
