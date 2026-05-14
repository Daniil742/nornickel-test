using Nornickel.Contracts.Enums;

namespace Nornickel.Contracts.Dtos.Login;

public class LoginResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public UserRoleDto Role { get; set; }
    public string Token { get; set; } = string.Empty;
}
