namespace FinanceHelper.Domain.Models;

public class BlobStorageOptions
{
  public string Endpoint { get; set; } = null!;
  public string AccessKey { get; set; } = null!;
  public string SecretKey { get; set; } = null!;
  public string Bucket { get; set; } = null!;
}