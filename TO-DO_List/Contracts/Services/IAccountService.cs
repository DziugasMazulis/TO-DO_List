using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Contracts.Services
{
    public interface IAccountService
    {
        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest forgotPassword);
        Task<IdentityResult> ResetPassword(string token, string email, ResetPasswordRequest resetPasswordRequest);
        Task<string> Login(LoginRequest loginDto);
    }
}
