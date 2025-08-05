using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookLibrarySystem.Middleware
{
    public class SessionKeyValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ServerSessionKey _serverSessionKey;
        public SessionKeyValidationMiddleware(RequestDelegate next, ServerSessionKey serverSessionKey)
        {
            _next = next;
            _serverSessionKey = serverSessionKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only check for authenticated users
            if (context.User.Identity.IsAuthenticated)
            {
                var claimKey = context.User.FindFirst("ServerSessionKey")?.Value;
                if (claimKey != _serverSessionKey.Value)
                {
                    await context.SignOutAsync();
                    context.Response.Redirect("/Pages/Login");
                    return;
                }
            }
            await _next(context);
        }
    }
}
