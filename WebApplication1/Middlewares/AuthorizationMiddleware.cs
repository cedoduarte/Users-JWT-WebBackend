namespace WebApplication1.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _validToken;

        public AuthorizationMiddleware(RequestDelegate next, string validToken)
        {
            _next = next;
            _validToken = validToken;
        }

        public async Task Invoke(HttpContext context)
        {
            string? foundToken = context.Request.Headers["Token"].FirstOrDefault()?.Split(" ").Last();
            if (foundToken is null || !string.Equals(foundToken, _validToken))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid token");
                return;
            }
            await _next(context);
        }
    }

    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorization(this IApplicationBuilder builder, string validToken)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>(validToken);
        }
    }
}
