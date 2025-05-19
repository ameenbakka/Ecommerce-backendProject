namespace Ecommerce.Models
{
    public class WishList
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int ProductId { get; set; }
            public virtual User? users { get; set; }
            public virtual Product? products { get; set; }
    }
}
