using AutoMapper;
using Ecommerce.DTO;
using Ecommerce.Models;
namespace Ecommerce.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<User, UserViewDto>().ReverseMap();
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<WishListDto, WishList>().ReverseMap();    
            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<CartItems, CartViewDto>().ReverseMap();
            CreateMap<Order, OrderViewDto>().ReverseMap();
            CreateMap<Address, createaddressdto>().ReverseMap();
            CreateMap<Address, ShowAddressDto>().ReverseMap();






        }
    }
}
