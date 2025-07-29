using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Options;
using FinanceHelper.Core.Messages;
using FinanceHelper.Domain.Dtos;
using FinanceHelper.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FinanceHelper.WebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UploadController(
  ITransactionRepository _transactionRepository,
  IMessageQueueService _messageQueueService
  ) : ControllerBase
{
  [HttpPost("csv")]
  public async Task<IActionResult> UploadCsv(IFormFile file)
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
        Description = record.Text,
        Amount = ParseSwedishAmount(record.Belopp),
      };
      await _transactionRepository.AddTransactionsAsync([tr]);
      var message = new TransactionMessage() { TransactionId = tr.Id };
      await _messageQueueService.PublishAsync(message);
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