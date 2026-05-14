using Nornickel.Database.DataModels;

namespace Nornickel.Infrastructure.Interfaces;

internal interface IJwtTokenService
{
    string GenerateToken(UserDataModel user);
}
