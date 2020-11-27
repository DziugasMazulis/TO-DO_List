using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Models;

namespace TO_DO_List.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<string> Login(LoginRequest loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return null;

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (passwordCheck == null &&
                !passwordCheck.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null)
                return null;
             
            var token = _jwtService.GenerateJwt(user, roles);

            if (token == null ||
                token.Length <= 0)
                return null;

            return token;
        }


        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
                return null;
             
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token == null)
                return null;
                
            var fotgotPasswordRespone = new ForgotPasswordResponse();
            fotgotPasswordRespone.Email = user.Email;
            fotgotPasswordRespone.Token = token;

            return fotgotPasswordRespone;
        }

        public async Task<IdentityResult> ResetPassword(string token, string email, ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return null;
             
            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordRequest.Password);
                return resetPassResult;
        }
    }
}
