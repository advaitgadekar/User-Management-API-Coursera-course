namespace UserManagementApi.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var started = DateTimeOffset.UtcNow;
        try
        {
            await _next(context);
        }
        finally
        {
            var elapsed = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs:F1} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsed);
        }
    }
}
