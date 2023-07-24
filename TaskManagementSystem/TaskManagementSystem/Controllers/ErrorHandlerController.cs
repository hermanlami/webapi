using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorHandlerController : ControllerBase
    {
        /// <summary>
        /// Kap exeptions qe hidhen nese nuk jane kapur dhe trajtuar ne BaseController.
        /// </summary>
        /// <param name="ex">Exception qe ka ndodhur.</param>
        [HttpGet]
        public IActionResult HandleError(Exception ex)
        {
            return Problem("An error occurred: " + ex.Message, statusCode: 500);
        }
    }
}
