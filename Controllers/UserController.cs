using System.Threading.Tasks;
using AutoMapper;
using EventManager.Dtos.User;
using EventManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManager.Controllers
{
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(
            UserManager<UserModel> userManager, 
            SignInManager<UserModel> signInManager, 
            ILogger<UserController> logger,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}