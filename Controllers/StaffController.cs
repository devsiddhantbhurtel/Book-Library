using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookLibrarySystem.Models;
using BookLibrarySystem.Services;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Staff")]
    public class StaffController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly StaffNotificationService _notificationService;
        private readonly ILogger<StaffController> _logger;

        public StaffController(
            ApplicationDbContext context,
            StaffNotificationService notificationService,
            ILogger<StaffController> logger)
        {
            _context = context;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost("verify-claim")]
        public async Task<IActionResult> VerifyClaimCode([FromBody] ClaimVerificationRequest request)
        {
            try
            {
                _logger.LogInformation($"Processing claim verification - ClaimCode: {request.ClaimCode}, MembershipID: {request.MembershipId}");

                // Find the order with the given claim code
                var order = await _context.Orders
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.ClaimCode == request.ClaimCode);

                if (order == null)
                {
                    _logger.LogWarning($"Invalid claim code: {request.ClaimCode}");
                    return BadRequest(new { success = false, message = "Invalid claim code." });
                }

                // Verify that the membership ID matches
                if (order.User?.MembershipID != request.MembershipId)
                {
                    _logger.LogWarning($"Membership ID mismatch - Expected: {order.User?.MembershipID}, Received: {request.MembershipId}");
                    return BadRequest(new { success = false, message = "Membership ID does not match the claim code." });
                }

                // Check if the order is already fulfilled
                if (order.Status == "Fulfilled")
                {
                    _logger.LogWarning($"Order already fulfilled - OrderID: {order.OrderID}");
                    return BadRequest(new { success = false, message = "This order has already been fulfilled." });
                }

                // Get the current staff member's ID
                var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _logger.LogInformation($"Staff member {staffId} processing order {order.OrderID}");

                var fulfillmentTime = DateTime.UtcNow;

                // Create a new staff claim record
                var staffClaimRecord = new StaffClaimRecord
                {
                    OrderID = order.OrderID,
                    StaffID = staffId,
                    ClaimTime = fulfillmentTime
                };

                // Update order status
                order.Status = "Fulfilled";

                // Save changes
                _context.StaffClaimRecords.Add(staffClaimRecord);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order {order.OrderID} marked as fulfilled");

                // Broadcast the notification
                await _notificationService.BroadcastOrderFulfillment(order, fulfillmentTime);
                _logger.LogInformation($"Notification broadcast completed for order {order.OrderID}");

                return Ok(new { 
                    success = true, 
                    message = "Order verified and marked as fulfilled.",
                    orderDetails = new {
                        orderDate = order.OrderDate,
                        totalAmount = order.TotalAmount,
                        customerName = order.User?.FullName
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing claim verification");
                return StatusCode(500, new { success = false, message = "An error occurred while processing the request." });
            }
        }
    }

    public class ClaimVerificationRequest
    {
        public string? ClaimCode { get; set; }
        public string? MembershipId { get; set; }
    }
} 