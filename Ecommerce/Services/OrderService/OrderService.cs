using System.Security.AccessControl;
using AutoMapper;
using Ecommerce.AppDbContext;
using Ecommerce.DTO;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppdbContext _context;
        private readonly IMapper _mapper;
        public OrderService(AppdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CreateOrder(int userid, CreateOrderDto AddOrder)
        {
            var usercart = await _context.carts.Include(x => x.CartItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(p => p.UserId == userid);
            if (usercart == null || usercart.CartItems == null)
            {
                return false;
            }
            foreach (var item in usercart.CartItems)
            {
                var isproducts = await _context.products.FirstOrDefaultAsync(x => x.Id == item.Product.Id);
                if (isproducts.Stock < item.Quantity)
                {
                    return false;
                }
                isproducts.Stock -= item.Quantity;
            }
            decimal? totalAmount = usercart.CartItems
                                  .Where(item => item.Product != null)
                                  .Sum(item => item.Product.Price * item.Quantity);
            var neworder = new Order
            {
                UserId = userid,
                OrderDate = DateTime.Now,
                AddressId = AddOrder.AddressId,
                TotalAmount = totalAmount,
                TransactionId = AddOrder.TransactionId,
                OrderItems = usercart.CartItems.Select(item =>
               new OrderItems
               {
                   ProductId = item.Product.Id,
                   Quantity = item.Quantity,
                   TotalPrice = (item.Product.Price * item.Quantity)

               }).ToList()
            };
            await _context.Orders.AddAsync(neworder);
             _context.carts.Remove(usercart);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ApiResponses<List<OrderViewDto>>> GetOrder(int userid)
        {

            var userOrders = await _context.Orders
                            .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                            .Where(x => x.UserId == userid)
                            .ToListAsync();
            var deliveryAddresses = await _context.Addresses
                .Where(x => x.UserId == userid)
                .ToListAsync();
            var addressDict = deliveryAddresses
                .GroupBy(addr => addr.UserId)
                .ToDictionary(group => group.Key, group => group.FirstOrDefault());

            var orderRes = userOrders.Select(order => new OrderViewDto
            {
                TransactionId = order.TransactionId,
                TotalAmount = order.TotalAmount,
                DeliveryAdrress = addressDict.TryGetValue(order.UserId, out var address)
                    ? $" {address.HouseName}, {address.Pincode} {address.LandMark}, {address.Place}"
                    : "Address not found",
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(orderItem => new OrderItemDto
                {
                    ProductName = orderItem.Product.Title,
                    TotalPrice = orderItem.TotalPrice,
                    Quantity = orderItem.Quantity
                }).ToList()
            }).ToList();

            return new ApiResponses<List<OrderViewDto>>(200, "Successfully Fetched User Orders", orderRes);

        }

        public async Task<List<AdminViewOrderDto>> GetOrdersForAdmin(int userid)
        {
            var userorders = await _context.users.Include(c => c.Orders).ThenInclude(n => n.OrderItems).FirstOrDefaultAsync(o => o.Id == userid);
            if (userorders == null)
            {
                return null;
            }
            var result = userorders.Orders.Select(item => new AdminViewOrderDto
            {
                OrderId = item.Id,
                TransactionId = item.TransactionId,
                TotalAmount = item.TotalAmount,
                OrderDate = item.OrderDate
            }).ToList();
            return result;
        }




        public async Task<int> TotalProductSold()
        {
            try
            {
                int sales = await _context.OrderItems.Where(oi => oi.Order.Status == "Delivered").SumAsync(x => x.Quantity);
                return sales;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<decimal?> TotalRevenue()
        {
            try
            {
                var total = await _context.OrderItems.Where(oi => oi.Order.Status == "Delivered").SumAsync(x => x.TotalPrice);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponses<string>> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return new ApiResponses<string>(404, "Order not found.");
            }

            order.Status = newStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return new ApiResponses<string>(200, "Order status updated successfully.");
        }
    }
}