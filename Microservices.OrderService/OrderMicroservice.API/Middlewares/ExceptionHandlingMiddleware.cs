namespace OrderMicroservice.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError("{Type}: {ExceptionMessage}", exception.GetType(), exception.Message);

            if (exception.InnerException is not null)
            {
                _logger.LogError("{Type}: {InnerExceptionMessage}", exception.InnerException.GetType(), exception.InnerException.Message);
            }

            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsJsonAsync(new
                { Message = exception.Message, Type = exception.GetType().ToString() });
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}