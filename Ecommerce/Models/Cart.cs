using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
