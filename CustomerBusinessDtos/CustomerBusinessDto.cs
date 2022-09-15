using System.ComponentModel.DataAnnotations;

namespace CustomerBusinessDtos
{
  public class CustomerBusinessDto
  {
    [Required]
    [MaxLength(100)]
    public string Name { get;  set; }

    [Required]
    [MaxLength(50)]
    public string RegistrationNumber { get;  set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get;  set; }

    [Required]
    [MaxLength(15)]
    public string Phone { get;  set; }

    [Required]
    [MaxLength(50)]
    public string Sur { get;  set; }

    [Required]
    [MaxLength(50)]
    public string First { get;  set; }

    [MaxLength(50)]
    public string? Middle { get;  set; }
  }
}