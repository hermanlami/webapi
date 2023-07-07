using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorHandlerController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError(CustomException ex)
        {
            return Problem("An error occurred: " + ex.Message, statusCode: 500);
        }
    }
}
