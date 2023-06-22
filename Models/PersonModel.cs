using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;

public class Person
{
  public int Id { get; set; }
  [Required, Display(Name = "First Name")]
  [StringLength(10, MinimumLength = 2)]
  public string? FirstName { get; set; }
  [Required, Display(Name = "Last Name")]
  [StringLength(10, MinimumLength = 2)]
  public string? LastName { get; set; }
  [Required]
  public string? Gender { get; set; }
  [Required]
  [Display(Name = "Date of Birth"), DataType(DataType.Date)]
  public DateTime DateOfBirth { get; set; }
  [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
  [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number")]
  public string? PhoneNumber { get; set; }
  [Display(Name = "Birth Place")]
  [StringLength(40, MinimumLength = 2)]
  public string? BirthPlace { get; set; }
  [Required, Display(Name = "Graduated")]
  public bool IsGraduated { get; set; }
  public string? FullName => $"{LastName} {FirstName}";
  public int Age => DateTime.Now.Year - DateOfBirth.Year;
}