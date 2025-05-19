using System.ComponentModel.DataAnnotations;
namespace Ecommerce.DTO
{
    public class AddProductDto
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int Stock {  get; set; }
        [Required]
        public int CategoryId {  get; set; }
    }
}
