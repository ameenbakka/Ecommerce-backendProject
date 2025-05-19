using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        [Url(ErrorMessage ="Invalid Format Of Url")]
        public string? Image {  get; set; }
        [Required]
        public int Stock {  get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category ? Category {  get; set; }
        public virtual List<CartItems> CartItems { get; set; }
    }
}

