namespace FinanceHelper.Domain.Models;

public record LabeledVector
{
  public string Category { get; init; } = string.Empty;
  public string Text { get; init; } = string.Empty;
  public float[] Embedding { get; init; } = [];
}