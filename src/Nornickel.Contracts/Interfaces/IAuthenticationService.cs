using Nornickel.Common;
using Nornickel.Contracts.Dtos.Login;

namespace Nornickel.Contracts.Interfaces;

public interface IAuthenticationService
{
    Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}
