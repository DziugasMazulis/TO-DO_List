using System.ComponentModel.DataAnnotations;
using TO_DO_List.Data;

namespace TO_DO_List.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(Constants.Password, ErrorMessage = Constants.NotMach)]
        public string ConfirmPassword { get; set; }
    }
}
