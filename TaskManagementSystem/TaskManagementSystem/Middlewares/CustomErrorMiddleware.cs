using Serilog;
using TaskManagementSystem.Common;

namespace TaskManagementSystem.Middlewares
{
    public class CustomErrorMiddleware : IMiddleware
    {
        /// <summary>
        /// Kap dhe trajton exceptions qe ndodhin nepermjet middleware.
        /// </summary>
        /// <param name="context">Konteksti i kerkeses.</param>
        /// <param name="next">Delegati qe perfaqeson middleware-in e rradhes ne pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Custom Error: " + ex.Message);
                Log.Error(ex.Message);
            }
        }
    }
}
