using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.BLL.Services;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class AuthenticationsController : BaseController
    {
        private readonly IAuthenticationsService _authenticationsService;
        public AuthenticationsController(IAuthenticationsService authenticationsService)
        {
            _authenticationsService = authenticationsService;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Mail.OnAccountCredentialsSent("lamiherman0@gmail.com");
                var response = await _authenticationsService.Authenticate(request);
                return Ok(response);
            });
        }

        [HttpPost]
        [Route("api/changePassword")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] UpdatePasswordRequest request)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = _authenticationsService.ChangePassword(id, request);
                return Ok(response);
            });
        }
    }
}
