using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Core.Messages;
using FinanceHelper.Domain.Dtos;
using FinanceHelper.Domain.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHelper.WebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UploadController(
  ITransactionRepository _transactionRepository,
  IPublishEndpoint _publishEndpoint
  ) : ControllerBase
{
  [HttpPost("csv")]
  public async Task<IActionResult> UploadCsv(IFormFile file, CancellationToken cancellationToken = default)
  {
    if (file == null || file.Length == 0)
    {
      return BadRequest("No file uploaded");
    }

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      Delimiter = ";"
    };

    using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
    using var csv = new CsvReader(reader, config);
    var records = csv.GetRecords<CsvTransactionRecordDto>().ToList();

    if (records.Count <= 0)
    {
      return Ok();
    }

    foreach (var record in records)
    {
      var tr = new TransactionRecord()
      {
        Date = DateTime.Parse(record.Datum).Date,
        Description = record.Text.Trim(),
        Amount = ParseSwedishAmount(record.Belopp),
      };
      await _transactionRepository.AddTransactionsAsync([tr]);
      await _publishEndpoint.Publish(new TransactionMessage { TransactionId = tr.Id }, cancellationToken);
    }
    return Created();
  }

  private static decimal ParseSwedishAmount(string amount)
  {
    var cleaned = amount.Replace("kr", "", StringComparison.OrdinalIgnoreCase)
                        .Replace(" ", "")
                        .Replace(",", ".")
                        .Replace("\u00A0", "");

    bool isNegative = cleaned.StartsWith('-');
    cleaned = cleaned.TrimStart('-', '+');

    if (decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
      return isNegative ? -value : value;

    throw new FormatException($"Could not parse amount: {amount}");
  }
}