using System.ComponentModel.DataAnnotations;
namespace Ecommerce.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

    }
}
