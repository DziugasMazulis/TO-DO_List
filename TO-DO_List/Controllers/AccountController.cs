using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Models;

namespace TO_DO_List.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;

        public AccountController(ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration config,
            IJwtService jwtService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                //exceptions handling and logging
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (passwordCheck.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        return Ok(_jwtService.GenerateJwt(user, roles));
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }

            return BadRequest();
        }
    }
}
