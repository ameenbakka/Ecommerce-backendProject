using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTO
{
    public class WishListDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
