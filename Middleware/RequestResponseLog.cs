
public class RequestResponseLog
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLog> _logger;

    public RequestResponseLog(RequestDelegate next, ILogger<RequestResponseLog> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Request
        _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);

        // Response
        await _next(context);

        _logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
    }
}