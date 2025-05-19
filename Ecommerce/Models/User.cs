using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Enter Proper Email")]
        public string? Email { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Password must be above 6 Characters")]
        [RegularExpression(@"^(?=.[A-Za-z])(?=.\d)(?=.[@$!%?&])[A-Za-z\d@$!%*?&]{8,}$",ErrorMessage ="Password must contain at least one letter, one number , and one special character.")] 
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsBlocked { get; set; }
        public virtual Cart? Cart { get; set; }

        public virtual List<Order>Orders { get; set; }
        public virtual List<WishList>? WishList { get; set; }
        public ICollection<Address>? Addresses { get; set; }


    }
}
