using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal? TotalAmount { get; set; }
        [Required]
        public int? AddressId { get; set; }

        [Required]
        public string TransactionId { get; set; }
        public virtual User? User { get; set; }
        public virtual List<OrderItems> OrderItems { get; set; }
        public string Status { get; set; }
        public virtual Address? Address { get; set; }

    }
}
