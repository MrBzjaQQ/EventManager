using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EventManager.Dtos.User;
using EventManager.Exceptions.Login;
using EventManager.Models;
using EventManager.Options.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManager.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _authenticationManager;

        public UserController(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            ILogger<UserController> logger,
            IJwtAuthenticationManager authenticationManager,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/signup")]
        public async Task<IActionResult> SignUp(UserCreateDto createUser)
        {
            UserModel model = _mapper.Map<UserModel>(createUser);
            var result = await _userManager.CreateAsync(model, createUser.Password);
            if (result.Succeeded)
            {
                return Ok(_mapper.Map<UserReadDto>(model));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("/signin")]
        public async Task<ActionResult> SignIn(UserLoginDto loginUser)
        {
            // TODO: fix logging
            try
            {
                UserLoginSuccessDto result = await _authenticationManager.Authenticate(loginUser);
                return Ok(result);
            }
            catch (AuthenticationFailedException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                _logger.LogDebug(ex.Message);
                return BadRequest(ModelState);
            }
            catch (JwtTokenCreationException ex)
            {
                foreach (var err in ex.Errors)
                {
                    _logger.LogError(err.Code, err.Description);
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("/whoami")]
        [Authorize]
        public async Task<IActionResult> WhoAmI()
        {
            // TODO: returns 401
            if (ModelState.IsValid)
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                UserModel user = await _authenticationManager.GetUserByToken(token);
                return Ok(_mapper.Map<UserReadDto>(user));
                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}