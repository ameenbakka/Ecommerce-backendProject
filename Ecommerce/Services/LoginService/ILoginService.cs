using Ecommerce.DTO;
namespace Ecommerce.Services.LoginService
{
    public interface ILoginService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<resultDto> Login(LoginDto loginDto);
    }
}
