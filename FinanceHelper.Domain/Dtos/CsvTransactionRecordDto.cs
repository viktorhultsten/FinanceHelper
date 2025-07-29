namespace FinanceHelper.Domain.Dtos;

public record CsvTransactionRecordDto
{
  public string Datum { get; init; } = string.Empty;
  public string Text { get; init; } = string.Empty;
  public string Belopp { get; init; } = string.Empty;
}