using Ecommerce.DTO;

namespace Ecommerce.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserViewDto>> GetAllUsers();
        Task<UserViewDto> GetUserById(int id);
        Task<bool> BlockandUnblock(int userid);
    }
}
