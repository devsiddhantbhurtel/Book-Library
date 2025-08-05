using Microsoft.AspNetCore.SignalR;
using BookLibrarySystem.Hubs;
using BookLibrarySystem.Models;
using Microsoft.Extensions.Logging;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Services
{
    public class StaffNotificationService
    {
        private readonly IHubContext<StaffNotificationHub> _hubContext;
        private readonly ILogger<StaffNotificationService> _logger;
        private readonly ApplicationDbContext _context;

        public StaffNotificationService(
            IHubContext<StaffNotificationHub> hubContext,
            ILogger<StaffNotificationService> logger,
            ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _logger = logger;
            _context = context;
        }

        public async Task BroadcastOrderFulfillment(Order order, DateTime fulfillmentTime)
        {
            try
            {
                _logger.LogInformation($"Broadcasting order fulfillment - OrderID: {order.OrderID}, ClaimCode: {order.ClaimCode}");
                
                // Create and save notification
                var notification = new StaffNotification
                {
                    OrderID = order.OrderID,
                    ClaimCode = order.ClaimCode,
                    FulfillmentTime = fulfillmentTime,
                    Type = "OrderFulfilled"
                };

                _context.StaffNotifications.Add(notification);
                await _context.SaveChangesAsync();

                var notificationData = new
                {
                    orderId = notification.OrderID,
                    claimCode = notification.ClaimCode,
                    fulfillmentTime = notification.FulfillmentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    type = notification.Type
                };

                await _hubContext.Clients.Group("StaffMembers").SendAsync("ReceiveNotification", notificationData);
                _logger.LogInformation("Order fulfillment notification sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting order fulfillment notification");
                throw;
            }
        }

        public async Task<List<object>> GetRecentNotifications(int count = 50)
        {
            try
            {
                _logger.LogInformation($"Retrieving {count} most recent notifications");
                
                var notifications = await _context.StaffNotifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(count)
                    .Select(n => new
                    {
                        orderId = n.OrderID,
                        claimCode = n.ClaimCode,
                        fulfillmentTime = n.FulfillmentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        type = n.Type
                    })
                    .ToListAsync();

                return notifications.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notifications");
                throw;
            }
        }
    }
} 