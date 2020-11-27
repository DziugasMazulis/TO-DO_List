using System;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Models;

namespace TO_DO_List.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailSender _emailSender;
        private readonly IAccountService _accountService;

        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJwtService jwtService,
            IEmailSender emailSender,
            IAccountService accountService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _emailSender = emailSender;
            _accountService = accountService;
        }

        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="loginRequest">A login request model containing of user email and password</param>
        /// <returns>On success JWT token</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var token = await _accountService.Login(loginRequest);

                if (token == null)
                    return NotFound();

                return Ok(token);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorRetrieveDatabase);
            }
        }

        /// <summary>
        /// Account recovery action sends password reset link to user's email
        /// </summary>
        /// <param name="forgotPasswordRequest">Forgot password request model containing user's email </param>
        /// <returns>On success sends password recovery link to user's mail</returns>
        [Authorize(Roles="user")]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var forgotPasswordResponse = await _accountService.ForgotPassword(forgotPasswordRequest);

                if (forgotPasswordResponse == null)
                    return NotFound(string.Format(Constants.UserNotFound, forgotPasswordRequest.Email));

                var callback = Url.Action(nameof(ResetPassword), Constants.Account, new { token = forgotPasswordResponse.Token, email = forgotPasswordResponse.Email }, Request.Scheme);
                var message = new Message(new string[] { forgotPasswordRequest.Email }, Constants.ResetPasswordToken, callback, null);
                await _emailSender.SendEmailAsync(message);

                return Ok(string.Format(Constants.PasswordResetLinkSent, forgotPasswordRequest.Email));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorPasswordResetEmail);
            }
            
        }

        /// <summary>
        /// Changes user's password
        /// </summary>
        /// <param name="resetPasswordRequest">Reset password request model containing of new password and it's confirmation</param>
        /// <param name="token">User's password reset token</param>
        /// <param name="email">User's email</param>
        /// <returns>Operation's success status</returns>
        [Authorize(Roles="user")]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest, string token, string email)
        {
            if (!ModelState.IsValid ||
                token == null ||
                 email == null)
                return BadRequest(new { resetPasswordRequest, token, email });

            try
            {
                var result = await _accountService.ResetPassword(token, email, resetPasswordRequest);

                if (result == null)
                    return NotFound(string.Format(Constants.UserNotFound, email));

                if(result.Succeeded)
                    return Ok(Constants.PasswordReset);

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                        Constants.InvalidToken);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorResetPassword);
            }
        }
    }
}
