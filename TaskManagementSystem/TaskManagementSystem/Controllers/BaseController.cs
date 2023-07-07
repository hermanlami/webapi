using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected async Task<IActionResult> HandleExceptionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (CustomException ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
