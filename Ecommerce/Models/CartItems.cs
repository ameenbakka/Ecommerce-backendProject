using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
}
