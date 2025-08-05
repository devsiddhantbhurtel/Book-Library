using BookLibrarySystem.Models;

namespace BookLibrarySystem.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}