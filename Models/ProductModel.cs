using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;

public class Product
{
  [Key, Required]
  public int ProductId { get; set; }
  [Required]
  public string? ProductName { get; set; }
  public string? Manufacture { get; set; }
  public List<Category> Categories { get; set; } = new();
}

public class ProductData
{
  public int ProductId { get; set; }
  public string? ProductName { get; set; }
  public string? Manufacture { get; set; }
  public List<CategoryData>? Categories { get; set; }
}