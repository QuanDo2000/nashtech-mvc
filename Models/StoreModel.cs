using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models;

public class ProductCategory
{
  [ForeignKey("Product"), Required]
  public int ProductId { get; set; }
  [ForeignKey("Category"), Required]
  public int CategoryId { get; set; }
}