using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManagementSystem.Common;
using TaskManagementSystem.Common.CustomExceptions;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Kap exceptions qe ndodhin ne metodat e controllers.
        /// </summary>
        /// <param name="action">Metoda we do ekzekutohet.</param>
        protected async Task<IActionResult> HandleExceptionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                int statusCode = 500;
                if (ex is NotFoundException)
                {
                    statusCode = 404;
                }
                if(ex is DuplicateInputException)
                {
                    statusCode = 409;
                }
                if(ex is FailedToAuthencitcateException)
                {
                    statusCode = 401;
                }
                if(ex is CustomException)
                {
                    statusCode = 422;
                }
                if(statusCode != 500)
                {
                    Log.Error(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
