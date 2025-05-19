using Ecommerce.DTO;
using Ecommerce.Models;
namespace Ecommerce.Services.CartService
{
    public interface ICartService
    {
        Task<ApiResponses<CartItems>> AddToCart(int productId, int userid);
        Task<List<CartViewDto>> GetCart(int userid);
        Task<bool>RemoveFromCart(int userid ,int productId);
        Task<bool> DecreaseQuantity(int userid, int productId);
    }
}
