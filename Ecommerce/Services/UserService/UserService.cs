using AutoMapper;
using Ecommerce.AppDbContext;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppdbContext _Context;
        private readonly IMapper _mapper;
        public UserService(AppdbContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }
        public async Task<List<UserViewDto>> GetAllUsers()
        {
            try
            {
                var users = await _Context.users.ToListAsync();
                if (users.Count > 0)
                {
                    var user = _mapper.Map<List<UserViewDto>>(users);
                    return user;
                }
                return new List<UserViewDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserViewDto> GetUserById(int id)
        {
            var user = await _Context.users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserViewDto>(user);
        }
        public async Task<bool> BlockandUnblock(int userid)
        {
            var user = await _Context.users.SingleOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                return false;
            }
            user.IsBlocked = !user.IsBlocked;
            await _Context.SaveChangesAsync();
            return true;
        }
        
    }
}
