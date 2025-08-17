using System.Net;
using System.Text.Json;

namespace HealthApp.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var error = new
            {
                StatusCode = context.Response.StatusCode,
                Message = _env.IsProduction()
                    ? "An unexpected error occurred. Please try again later."
                    : ex.Message
            };

            var payload = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(payload);
        }
    }
}