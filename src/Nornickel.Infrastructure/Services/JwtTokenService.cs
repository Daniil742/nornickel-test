using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nornickel.Contracts.Configurations;
using Nornickel.Database.DataModels;
using Nornickel.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nornickel.Infrastructure.Services;

internal class JwtTokenService(
    IOptions<JwtTokenConfiguration> configuration
    ) : IJwtTokenService
{
    private readonly JwtTokenConfiguration _configuration = configuration.Value;

    public string GenerateToken(UserDataModel user)
    {
        var expireMinutes = Convert.ToDouble(_configuration.ExpiresInMinutes);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
