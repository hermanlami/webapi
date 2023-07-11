using TaskManagementSystem.Common;

namespace TaskManagementSystem.Middlewares
{
    public class CustomErrorMiddleware : IMiddleware
    {
        private readonly ILogger<CustomErrorMiddleware> _logger;
        public CustomErrorMiddleware(ILogger<CustomErrorMiddleware> logger)
        {

            _logger = logger;

        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Custom Error: " + ex.Message);
                _logger.LogError(ex.Message);
            }
        }
    }
}
