using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookLibrarySystem.Services;
using BookLibrarySystem.Models;
using System.Security.Claims;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly BookLibrarySystem.ServerSessionKey _serverSessionKey;

        public UserController(IUserService userService, IJwtService jwtService, BookLibrarySystem.ServerSessionKey serverSessionKey)
        {
            _userService = userService;
            _jwtService = jwtService;
            _serverSessionKey = serverSessionKey;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            Console.WriteLine($"LoginModel: Email={model?.Email}, Password={model?.Password}");
            var user = await _userService.AuthenticateAsync(model.Email, model.Password);
            Console.WriteLine("AuthenticateAsync returned: " + (user != null ? "User found" : "null"));
            if (user == null)
                return Unauthorized();

            // Create the claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("ServerSessionKey", _serverSessionKey.Value)
            };

            Console.WriteLine($"Setting claims for user {user.Email} with role {user.Role}");

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                scheme: "Cookies",
                principal: new ClaimsPrincipal(claimsIdentity),
                properties: authProperties);

            Console.WriteLine($"Login successful for user {user.Email}");
            return Ok(new { role = user.Role });
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }
            try
            {
                var user = new User
                {
                    Email = model.Email,
                    FullName = model.FullName, // Changed from Name to FullName
                    Role = "User"
                };

                var result = await _userService.RegisterAsync(user, model.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("REGISTRATION ERROR: " + ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<User>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new {
    fullName = user.FullName,
    email = user.Email,
    role = user.Role
});
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        [Authorize]
        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            // Only allow updating FullName and Email
            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _userService.UpdateAsync(user);
            return Ok(new {
                fullName = user.FullName,
                email = user.Email,
                role = user.Role
            });
        }
    }
}