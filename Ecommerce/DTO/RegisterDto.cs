using System.ComponentModel.DataAnnotations;
namespace Ecommerce.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Enter Proper Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(6, ErrorMessage = "Password must be above 6 Characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
    ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]

        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not Match.")]
        public string? ConfrimPassword { get; set; }
    }
}
