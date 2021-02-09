using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EventManager.Options.Tokens
{
    // TODO: move to the appsettings.json
    public class BearerOptions
    {
        public const string ISSUER = "Event Manager Server";
        public const string AUDIENCE = "Event Manager Client";
        public const int LIFETIME = 25200;
        public const string TOKEN_NAME = "AuthenticationToken";
        const string KEY = "pa55word_secretkey!123";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}