namespace FinanceHelper.Application.Interfaces;

public interface ICategorizerService
{
  public Task<string> CategorizeAsync(string text);
}