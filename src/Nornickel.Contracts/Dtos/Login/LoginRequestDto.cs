using System.ComponentModel.DataAnnotations;

namespace Nornickel.Contracts.Dtos.Login;

public class LoginRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
