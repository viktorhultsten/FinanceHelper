namespace FinanceHelper.Domain.Models;

public class CategoryDefinition
{
  public string Name { get; set; } = string.Empty;
  public List<string> Keywords { get; set; } = [];
}