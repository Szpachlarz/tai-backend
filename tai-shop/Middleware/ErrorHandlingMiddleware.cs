using System.Text.Json;
using tai_shop.Exceptions;

namespace tai_shop.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case NotFoundException:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case ArgumentException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case InvalidOperationException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                default:
                    _logger.LogError(ex, "An unexpected error occurred");
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = ex.Message });
            await response.WriteAsync(result);
        }
    }
}
