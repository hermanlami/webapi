using Serilog;
using TaskManagementSystem.Common;
using TaskManagementSystem.Common.CustomExceptions;

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
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, 404);
            }
            catch (DuplicateInputException ex)
            {
                await HandleExceptionAsync(context, ex, 409);
            }
            catch (FailedToAuthencitcateException ex)
            {
                await HandleExceptionAsync(context, ex, 401);
            }
            catch (CustomException ex)
            {
                await HandleExceptionAsync(context, ex, 422);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, 500);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync("Custom Error: " + ex.Message);

            if (statusCode != 500)
            {
                Log.Error(ex.Message);
            }
        }
    }
}