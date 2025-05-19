using Ecommerce.DTO;
using Ecommerce.Models;

namespace Ecommerce.Services.OrderService
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(int userid, CreateOrderDto AddOrder );
        Task<ApiResponses<List<OrderViewDto>>> GetOrder(int userid);
        Task<List<AdminViewOrderDto>> GetOrdersForAdmin(int userid);
        Task<int> TotalProductSold();
        Task<decimal?> TotalRevenue();
        Task<ApiResponses<string>> UpdateOrderStatus(int OrderId, string NewStatus);
    }
}
