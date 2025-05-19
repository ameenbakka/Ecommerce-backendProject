using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.Services.WishListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _service;
        public WishListController(IWishListService service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddtoWishlist(int productid)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            string isadded = await _service.AddToWishList(userId, productid);
            if (isadded == "item added To whish list")
            {
                return Ok("The product is added to wishlist");
            }
            else
            {
                return BadRequest("Wrong item or item already in the wishlist");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> RemovefromWishlist(int productid)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            bool isadded = await _service.RemoveFromWishlist(userId, productid);
            if (isadded)
            {
                return Ok("The product is removed from wishlist");
            }
            else
            {
                return BadRequest("Item not found in the wish list");
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWhishLists()
        {
            try
            {

                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.GetWishList(userId);

                return Ok(new ApiResponses<List<WishListViewDto>>(200, "Whishlist fetched successfully", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Failed to fetch wishlist", null, ex.Message));
            }
        }

    }
}