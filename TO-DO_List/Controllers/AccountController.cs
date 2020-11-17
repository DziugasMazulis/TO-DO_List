using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Models;

namespace TO_DO_List.Controllers
{
    [Route("/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //move logic to service
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                        if (passwordCheck != null &&
                            passwordCheck.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user);

                            if(roles != null)
                            {
                                return Ok(_jwtService.GenerateJwt(user, roles));
                            }
                        }
                    }

                    return NotFound();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database");
                }
            }

            return BadRequest();
        }
    }
}
