using System.Text.Json;

namespace MoviesChallenge.Api.Helpers;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiSecret;

    public TokenMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiSecret = $"Bearer {configuration["ApiSecret"]}" ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip validation for safe methods GET (e.g., GetByUniqueId, Searchs)
        if (HttpMethods.IsGet(context.Request.Method))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var token) || token != _apiSecret)
        {
            var response = new { statusCode = StatusCodes.Status401Unauthorized, message = "Unauthorized: Invalid API Secret" };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(json);
            return;
        }
        await _next(context);
    }
}