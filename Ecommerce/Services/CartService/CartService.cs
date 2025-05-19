using Ecommerce.AppDbContext;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Ecommerce.DTO;
namespace Ecommerce.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly AppdbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;
        public CartService(AppdbContext context, IMapper mapper, ILogger<CartService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponses<CartItems>> AddToCart(int productId, int userid)
        {
            try
            {
                var isuser = await _context.users.Include(ci => ci.Cart).ThenInclude(x => x.CartItems).FirstOrDefaultAsync(x => x.Id == userid);
                if (isuser == null)
                {
                    return new ApiResponses<CartItems>(404, "User not found");
                }
                var isproduct = await _context.products.FirstOrDefaultAsync(x => x.Id == productId);
                if (isproduct == null)
                {
                    return new ApiResponses<CartItems>(404, "Product not found");
                }
                if (isuser.Cart == null)
                {
                    isuser.Cart = new Cart()
                    {
                        UserId = userid,
                        CartItems = new List<CartItems>()
                    };
                    await _context.carts.AddAsync(isuser.Cart);
                    await _context.SaveChangesAsync();
                }
                if (isproduct?.Stock <= 0)
                {
                    return new ApiResponses<CartItems>(404, "Out of stock");
                }
                var check = isuser.Cart?.CartItems?.FirstOrDefault(p => p.Id == productId);
                if (check != null)
                {
                    check.Quantity++;
                    await _context.SaveChangesAsync();

                }
                else
                {

                    var item = new CartItems
                    {
                        CartId = isuser.Cart.Id,
                        Id = productId,
                        Quantity = 1
                    };
                    isuser?.Cart?.CartItems?.Add(item);
                }
                await _context.SaveChangesAsync();
                return new ApiResponses<CartItems>(200, "Successfully added to cart");

            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding to cart: " + ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: " + ex.InnerException.Message);
                }
                return new ApiResponses<CartItems>(500, "Internal server error", null, ex.Message);
            }
        }
        public async Task<List<CartViewDto>> GetCart(int userid)
        {
            if (userid == 0)
            {
                throw new Exception("Userid is null");
            }
            var cart = await _context.carts.Include(c => c.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(x => x.UserId == userid);
            if (cart != null)
            {
                var cartitem = cart.CartItems.Select(x => new CartViewDto
                {
                    ProductId = x.Id,
                    ProductName = x.Product.Title,
                    Price = x.Product.Price,
                    Quantity = x.Quantity,
                    TotalAmount = x.Quantity * x.Product.Price,
                    Image = x.Product.Image

                }).ToList();
                return cartitem;
            }
            return new List<CartViewDto>();
        }
        public async Task<bool> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var user = await _context.users.Include(c => c.Cart)
                                    .ThenInclude(ci => ci.CartItems)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User is not found");
                }

                var deleteItem = user?.Cart?.CartItems?.FirstOrDefault(p => p.Id == productId);
                if (deleteItem == null)
                {
                    return false;
                }

                user?.Cart?.CartItems?.Remove(deleteItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DecreaseQuantity(int userid, int productid)
        {
            try
            {
                var user = await _context.users.Include(c => c.Cart)
                                    .ThenInclude(ci => ci.CartItems)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefaultAsync(u => u.Id == userid);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var item = user?.Cart?.CartItems?.FirstOrDefault(p => p.Id == productid);
                if (item == null)
                {
                    return false;

                }
                if (item.Quantity > 1)
                {
                    item.Quantity--;

                }
                else
                {
                    item.Quantity = 1;
                }
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
