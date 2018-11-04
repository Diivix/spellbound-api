using System.ComponentModel.DataAnnotations;

namespace spellbound_api.Models
{
  public class LoginDto
  {
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
  }
}