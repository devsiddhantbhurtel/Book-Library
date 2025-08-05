using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BookLibrarySystem.Hubs
{
    [Authorize(Roles = "Staff")]
    public class StaffNotificationHub : Hub
    {
        private readonly ILogger<StaffNotificationHub> _logger;

        public StaffNotificationHub(ILogger<StaffNotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = Context.User;
                _logger.LogInformation($"Staff member connected. User: {user?.Identity?.Name}, ConnectionId: {Context.ConnectionId}");
                
                await Groups.AddToGroupAsync(Context.ConnectionId, "StaffMembers");
                _logger.LogInformation($"Added connection {Context.ConnectionId} to StaffMembers group");
                
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync");
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                _logger.LogInformation($"Staff member disconnected. ConnectionId: {Context.ConnectionId}");
                
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "StaffMembers");
                _logger.LogInformation($"Removed connection {Context.ConnectionId} from StaffMembers group");
                
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync");
                throw;
            }
        }
    }
} 