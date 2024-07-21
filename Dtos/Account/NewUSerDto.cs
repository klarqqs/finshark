using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account;

public class NewUSerDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}