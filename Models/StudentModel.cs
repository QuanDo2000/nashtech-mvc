using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;

public class Student
{
  [Key, Required]
  public int StudentId { get; set; }
  [Required]
  public string? FirstName { get; set; }
  [Required]
  public string? LastName { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
}