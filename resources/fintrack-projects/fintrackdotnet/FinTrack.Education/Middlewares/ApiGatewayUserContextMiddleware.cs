using System.Security.Claims;

namespace FinTrack.Education.Middlewares;

public class ApiGatewayUserContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiGatewayUserContextMiddleware> _logger;

    public ApiGatewayUserContextMiddleware(RequestDelegate next, ILogger<ApiGatewayUserContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
        var roles = context.Request.Headers["X-User-Roles"].FirstOrDefault()?.Split(',') ?? Array.Empty<string>();

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Missing X-User-Id header");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        // Build a ClaimsPrincipal
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, "ApiGateway");
        context.User = new ClaimsPrincipal(identity);

        await _next(context);
    }
}
