using BookLibrarySystem.Models;
using BookLibrarySystem.Repositories;

namespace BookLibrarySystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;

        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Validate stock availability
            foreach (var item in order.OrderItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);
                if (book.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for book: {book.Title}");
            }

            // Calculate total and discounts
            decimal total = 0;
            int totalBooks = 0;
            foreach (var item in order.OrderItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);
                total += book.Price * item.Quantity;
                totalBooks += item.Quantity;
            }
            bool hasFivePlusDiscount = totalBooks >= 5;
            bool hasLoyaltyDiscount = false;
            if (order.UserID > 0)
            {
                var userOrders = await _orderRepository.GetOrdersByUserIdAsync(order.UserID);
                int successfulOrders = userOrders.Count(o => o.Status == "Fulfilled");
                hasLoyaltyDiscount = (successfulOrders > 0 && successfulOrders % 10 == 0);
            }
            order.HasFivePlusDiscount = hasFivePlusDiscount;
            order.HasLoyaltyDiscount = hasLoyaltyDiscount;
            decimal discount = 1.0m;
            if (hasFivePlusDiscount) discount *= 0.95m;
            if (hasLoyaltyDiscount) discount *= 0.90m;
            order.TotalAmount = Math.Round(total * discount, 2);
            
            // Generate claim code
            order.ClaimCode = GenerateClaimCode();
            
            // Set initial status
            order.Status = "Pending";
            order.OrderDate = DateTime.UtcNow;

            // Use CreateOrderAsync instead of AddOrderAsync
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Update stock quantities
            foreach (var item in order.OrderItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);
                book.StockQuantity -= item.Quantity;
                await _bookRepository.UpdateBookAsync(book);
            }

            return createdOrder;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.Status != "Pending")
                return false;

            // Restore stock quantities
            foreach (var item in order.OrderItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);
                book.StockQuantity += item.Quantity;
                await _bookRepository.UpdateBookAsync(book);
            }

            order.Status = "Cancelled";
            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return 0;

            decimal total = 0;
            foreach (var item in order.OrderItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookID);
                total += book.Price * item.Quantity;
            }

            // Apply discounts if applicable
            if (order.HasFivePlusDiscount)
                total *= 0.95m;
            if (order.HasLoyaltyDiscount)
                total *= 0.90m;

            return total;
        }

        private string GenerateClaimCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}