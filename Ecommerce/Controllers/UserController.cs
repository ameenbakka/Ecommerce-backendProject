using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound(new ApiResponses<string>(404, "User Not Found", null));
            var res = new ApiResponses<UserViewDto>(200, "Fetched User by Id", user);
            return Ok(res);
        }
        [HttpPatch("{id}/BlockUnblock")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> BlockorUnbolock (int id)
        {
            try
            {
                bool isblocked = await _userService.BlockandUnblock(id);
                return Ok(isblocked);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
