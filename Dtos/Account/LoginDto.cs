using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account;

public class LoginDto
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}