using System.Dynamic;
using Microsoft.IdentityModel.Tokens;

namespace EventManager.Options.Tokens.Jwt
{
    public interface IJwtOptions
    {
        string ISSUER { get; init; }
        string AUDIENCE { get; init; }
        int LIFETIME { get; init; }
        string TOKEN_NAME { get; init; }
        SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}