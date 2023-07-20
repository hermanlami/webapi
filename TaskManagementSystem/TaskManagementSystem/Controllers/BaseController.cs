using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common;

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
            catch (CustomException ex)
            {
               return BadRequest(ex.Message);
            }
        }
    }
}
