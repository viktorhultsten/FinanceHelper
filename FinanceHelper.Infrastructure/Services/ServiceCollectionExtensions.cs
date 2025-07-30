using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using FinanceHelper.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceHelper.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddMinioClient(this IServiceCollection services, IConfiguration config)
  {
    var opts = new BlobStorageOptions();
    config.GetSection("Minio").Bind(opts);
    services.AddSingleton(opts);

    services.AddSingleton<IAmazonS3>(sp =>
    {
      var mo = sp.GetRequiredService<BlobStorageOptions>();
      var creds = new BasicAWSCredentials(mo.AccessKey, mo.SecretKey);
      var cfg = new AmazonS3Config
      {
        ServiceURL = mo.Endpoint,
        ForcePathStyle = true,
      };
      return new AmazonS3Client(creds, cfg);
    });

    return services;
  }
}