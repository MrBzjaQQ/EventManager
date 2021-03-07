using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventManager.Dtos.User;
using EventManager.Exceptions.Login;
using EventManager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EventManager.Options.Tokens.Jwt
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IJwtOptions _jwtOptions;

        public JwtAuthenticationManager(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            IJwtOptions jwtOptions
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions;
        }

        public async Task<UserLoginSuccessDto> Authenticate(UserLoginDto loginUser)
        {
            // TODO: if there is a valid token, we could just return it
            // await _userManager.GetAuthenticationTokenAsync()
            UserModel user = await _userManager.FindByNameAsync(loginUser.UserName);

            if (user == null)
            {
                throw new AuthenticationFailedException();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, true, false);

            if (!result.Succeeded)
            {
                throw new AuthenticationFailedException();
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.ISSUER,
                audience: _jwtOptions.AUDIENCE,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(_jwtOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256),
                claims: new[] {new Claim(ClaimTypes.Name, user.UserName)}
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenResult = await _userManager.SetAuthenticationTokenAsync(user, _jwtOptions.ISSUER,
                _jwtOptions.TOKEN_NAME, token);

            if (!tokenResult.Succeeded)
            {
                throw new JwtTokenCreationException(errors: tokenResult.Errors);
            }
            
            return new UserLoginSuccessDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                AccessToken = token
            };
        }

        public async Task<UserModel> GetUserByToken(string token)
        {
            JwtSecurityToken jwtToken = new JwtSecurityToken(token);
            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            
            if (userName == null)
            {
                // TODO: do not return null
                return null;
            }

            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }
    }
}