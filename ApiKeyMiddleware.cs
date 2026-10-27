namespace UserManagementApi.Middleware;

public sealed class ApiKeyMiddleware
{
    private const string HeaderName = "X-API-Key";
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var expectedKey = _configuration["API_KEY"];

        // A missing key configuration fails closed for write operations.
        if (string.IsNullOrWhiteSpace(expectedKey))
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Write operations are disabled until the API_KEY environment variable is configured."
            });
            return;
        }

        if (!context.Request.Headers.TryGetValue(HeaderName, out var suppliedKey) ||
            !string.Equals(suppliedKey.ToString(), expectedKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = "A valid X-API-Key header is required." });
            return;
        }

        await _next(context);
    }
}
