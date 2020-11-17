using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
