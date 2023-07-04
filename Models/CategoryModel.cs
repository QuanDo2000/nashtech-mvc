using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;

public class Category
{
  [Key, Required]
  public int CategoryId { get; set; }
  [Required]
  public string? CategoryName { get; set; }
  public virtual List<Product> Products { get; set; } = new();
}

public class CategoryData
{
  public int CategoryId { get; set; }
  public string? CategoryName { get; set; }
  public List<ProductData>? Products { get; set; }
}