using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nornickel.Common;
using Nornickel.Contracts.Dtos.Login;
using Nornickel.Contracts.Enums;
using Nornickel.Contracts.Interfaces;
using Nornickel.Database.Contexts;
using Nornickel.Database.DataModels;
using Nornickel.Infrastructure.Interfaces;

namespace Nornickel.Infrastructure.Services;

internal class AuthenticationService(
    IDbContextFactory<HrSystemDbContext> dbContextFactory,
    IJwtTokenService jwtTokenService,
    IPasswordHasher<UserDataModel> passwordHasher
    ) : IAuthenticationService
{
    private readonly IDbContextFactory<HrSystemDbContext> _dbContextFactory = dbContextFactory;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IPasswordHasher<UserDataModel> _passwordHasher = passwordHasher;

    public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var user = await context.Set<UserDataModel>()
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null ||
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            return ServiceResult<LoginResponseDto>.Fail(new Dictionary<string, string[]>
            {
                { "Credentials", new[] { "Неверный логин или пароль" } }
            }, "Аутентификация не удалась");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return ServiceResult<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Message = "Авторизация успешна",
            Username = user.Username,
            Role = (UserRoleDto)user.Role,
            Token = token
        });
    }
}
