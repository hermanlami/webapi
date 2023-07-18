using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Common;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class AuthenticationsController : BaseController
    {
        private readonly IAuthenticationsService _authenticationsService;
        private readonly ITokensService _tokensService;
        public AuthenticationsController(IAuthenticationsService authenticationsService, ITokensService tokensService)
        {
            _authenticationsService = authenticationsService;
            _tokensService = tokensService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
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

        [HttpPost]
        [AllowAnonymous]
        [Route("api/refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tokenResponse = _tokensService.RefreshToken(refreshToken);
                return Ok(tokenResponse);
            });
        }
    }
}
