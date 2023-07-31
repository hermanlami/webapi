using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Common;
using TaskManagementSystem.Middlewares;

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
        /// <summary>
        /// Kryen autentifikimin e perdoruesit.
        /// </summary>
        /// <param name="request">Modeli qe permban kredencialet</param>
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
        /// <summary>
        /// Ndryshon password-in e perdoruesit.
        /// </summary>
        /// <param name="id">Id e perdoruesit.</param>
        /// <param name="request">Modeli qe permban password-in e ri dhe ate te vjeter.</param>
        [HttpPost]
        [Route("api/changePassword")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager", "Developer" } })]

        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Int32.TryParse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out int id);
                var response = await _authenticationsService.ChangePassword(id, request);
                return Ok(response);
            });
        }
        /// <summary>
        /// Ben refresh token-in e saposkaduar duke perdorur nje refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh tokne i nevojshem per te bere refresh token-in e skaduar.</param>
        [HttpPost]
        [Route("api/refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tokenResponse = await _tokensService.RefreshToken(refreshToken);
                return Ok(tokenResponse);
            });
        }
    }
}
