using Ecommerce.AppDbContext;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppdbContext _Context;
        private readonly ICartService _service;
        public CartController(AppdbContext context, ICartService service)
        {
            _Context = context;
            _service = service;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddtoCart(int productid)
        {
            if (HttpContext.Items["UserId"] is not int userid)
            {
                return Unauthorized("Invalid or missing user information.");
            }

            var isAdded = await _service.AddToCart(productid, userid);
            if (isAdded.StatusCode == 200)
            {
                return Ok(isAdded);
            }
            else if (isAdded.StatusCode == 404)
            {
                return NotFound(new ApiResponses<string>(404, isAdded.Message));
            }
            else if (isAdded.StatusCode == 409)
            {
                return Conflict(new ApiResponses<string>(409, isAdded.Message));
            }
            return BadRequest(new ApiResponses<string>(400, "Bad request"));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            int userid = Convert.ToInt32(HttpContext.Items["UserId"]);
            var cartitems = await _service.GetCart(userid);
            if (cartitems.Count == 0)
            {
                return Ok(new ApiResponses<IEnumerable<CartViewDto>>(200, "Cart is empty", cartitems));
            }
            return Ok(new ApiResponses<IEnumerable<CartViewDto>>(200, "Cart successfully fetched", cartitems));
        }
        [HttpDelete("Delete/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                bool res = await _service.RemoveFromCart(userId, productId);
                if (res == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item is not found in cart", null));
                }
                return Ok(new ApiResponses<string>(200, "Item successfully deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }
        [HttpPut("DecreaseQuantity")]
        [Authorize]
        public async Task<IActionResult> DecreaseQuantity(int prductid)
        {
            try
            {
                int userid = Convert.ToInt32(HttpContext.Items["UserId"]);
                bool items = await _service.DecreaseQuantity(userid, prductid);
                if (items == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item not found in the cart", null, "Item not found in the cart"));
                }
                return Ok(new ApiResponses<string>(200, "Qty decreased"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}