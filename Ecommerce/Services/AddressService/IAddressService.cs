using Ecommerce.Models;
using Ecommerce.DTO;

namespace Ecommerce.Services.AddressService
{
    public interface IAddressService
    {
        Task<ApiResponses<string>> AddAdress(int userid, createaddressdto addaddress);
        Task<ApiResponses<List<ShowAddressDto>>> ShowAddresses(int userid);
        Task<bool> DeleteAddress(int userid, int addressid);
    }
}
