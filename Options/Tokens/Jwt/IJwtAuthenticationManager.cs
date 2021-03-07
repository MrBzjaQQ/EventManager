using System.Threading.Tasks;
using EventManager.Dtos.User;
using EventManager.Models;

namespace EventManager.Options.Tokens.Jwt
{
    public interface IJwtAuthenticationManager
    {
        Task<UserLoginSuccessDto> Authenticate(UserLoginDto loginUser);

        Task<UserModel> GetUserByToken(string token);
    }
}