using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventManager.Options.Tokens.Jwt
{
    // TODO: move to the appsettings.json
    public class JwtOptions: IJwtOptions
    {
        private readonly string _key;

        public JwtOptions(IConfiguration configuration)
        {
            // TODO: use configuration to fill the fields
            ISSUER = configuration.GetValue<string>("JwtToken:Issuer");
            AUDIENCE = configuration.GetValue<string>("JwtToken:Audience");
            LIFETIME = configuration.GetValue<int>("JwtToken:Lifetime");
            TOKEN_NAME = configuration.GetValue<string>("JwtToken:TokenName");
            _key = configuration.GetValue<string>("JwtToken:SecretKey");
        }
        public string ISSUER { get; init; }
        public string AUDIENCE { get; init; }
        public int LIFETIME { get; init; }
        public string TOKEN_NAME { get; init; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}