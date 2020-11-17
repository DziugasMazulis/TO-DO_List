using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
