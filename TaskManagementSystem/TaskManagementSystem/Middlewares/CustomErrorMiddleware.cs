using TaskManagementSystem.Common;

namespace TaskManagementSystem.Middlewears
{
    public class CustomErrorMiddleware : IMiddleware
    {
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
            }
        }
    }
}
