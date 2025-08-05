using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Add this line
using Microsoft.AspNetCore.Authorization;
using BookLibrarySystem.Services;
using BookLibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Cookies")]
public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly EmailService _emailService;
        private readonly BookLibrarySystem.Data.ApplicationDbContext _context;

        public OrderController(IOrderService orderService, BookLibrarySystem.Data.ApplicationDbContext context)
        {
            _orderService = orderService;
            _context = context;
            // Configured for Gmail SMTP
            _emailService = new EmailService(
                smtpHost: "smtp.gmail.com",
                smtpPort: 587,
                smtpUser: "bib0271@my.londonmet.ac.uk",
                smtpPass: "nhrinvcaetivduyg",
                fromEmail: "bib0271@my.londonmet.ac.uk"
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                order.UserID = userId;
                var result = await _orderService.CreateOrderAsync(order);

                // Clear user's cart
                var cartItems = _context.CartItems.Where(x => x.UserID == userId);
                _context.CartItems.RemoveRange(cartItems);
                _context.SaveChanges();

                // Prepare bill details
                string discountDetails = "";
                if (result.HasFivePlusDiscount)
                    discountDetails += "<div style='color:green'>5% multi-book discount applied.</div>";
                if (result.HasLoyaltyDiscount)
                    discountDetails += "<div style='color:green'>10% loyalty discount applied.</div>";
                var bill = "<h3>Order Confirmation</h3>" +
                    $"<p>Thank you for your order, Claim Code: <b>{result.ClaimCode}</b></p>" +
                    discountDetails +
                    "<ul>" +
                    string.Join("", result.OrderItems.Select(oi => $"<li>{oi.Quantity} x {oi.Book?.Title ?? "Book #" + oi.BookID} @ ${oi.UnitPrice:F2} = ${oi.TotalPrice:F2}</li>")) +
                    "</ul>" +
                    $"<p><b>Total: ${result.TotalAmount:F2}</b></p>";

                // Send confirmation email
                var userEmail = _context.Users.FirstOrDefault(u => u.UserID == userId)?.Email;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    await _emailService.SendEmailAsync(userEmail, "Order Confirmation", bill);
                }

                return Ok(new { success = true, claimCode = result.ClaimCode });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (order.UserID != userId && !User.IsInRole("Admin"))
                return Forbid();

            return Ok(order);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderID == orderId && o.UserID == userIdInt);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status != "Pending")
            {
                return BadRequest("Only pending orders can be cancelled.");
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{orderId}/items/{orderItemId}/cancel")]
        public async Task<IActionResult> CancelOrderItem(int orderId, int orderItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderID == orderId && o.UserID == userIdInt);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            if (order.Status != "Pending")
            {
                return BadRequest("Only items in pending orders can be cancelled.");
            }

            var orderItem = order.OrderItems?.FirstOrDefault(oi => oi.OrderItemID == orderItemId);
            if (orderItem == null)
            {
                return NotFound("Order item not found");
            }

            // Remove the order item
            _context.OrderItems.Remove(orderItem);

            // Recalculate order total
            order.TotalAmount = order.OrderItems.Where(oi => oi.OrderItemID != orderItemId)
                                              .Sum(oi => oi.Quantity * oi.UnitPrice);

            // If no items left, cancel the entire order
            if (!order.OrderItems.Any(oi => oi.OrderItemID != orderItemId))
            {
                order.Status = "Cancelled";
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}