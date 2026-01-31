using System.Security.Claims;

namespace LoanSystem.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log Request Path + Time
            Console.WriteLine($"Request: {context.Request.Path} at {DateTime.Now}");

            // Log Authenticated User
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"User: {userId}");
            }

            // Add Custom Response Header
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("X-Custom-Header", "Processed by LoanSystem Middleware");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
