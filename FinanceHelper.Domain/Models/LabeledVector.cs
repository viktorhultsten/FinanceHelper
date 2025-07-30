using System.ComponentModel.DataAnnotations;
using Pgvector;

namespace FinanceHelper.Domain.Models;

public class LabeledVector
{
  [Key]
  public int Id { get; set; }
  public string Category { get; set; } = string.Empty;
  public string Text { get; set; } = string.Empty;
  public Vector? Embedding { get; set; }
}