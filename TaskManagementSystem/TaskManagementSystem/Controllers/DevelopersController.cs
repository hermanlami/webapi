using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Middlewares;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class DevelopersController : BaseController
    {
        private readonly IDevelopersService _developersService;
        private readonly TokenService _tokenService;
        private readonly ILogger<DevelopersController> _logger;
        public DevelopersController(IDevelopersService developersService, TokenService tokenService, ILogger<DevelopersController> logger)
        {
            _developersService = developersService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost, Authorize]
        [Route("api/developers")]
        public async Task<IActionResult> AddDeveloper([FromBody] Developer model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.AddDeveloper(model);
                if (developer.Id > 0)
                {
                    _logger.LogInformation("Developer added successfully");
                    return Ok(developer);
                }
                else
                {
                    _logger.LogError("Developer could not be added");
                    return BadRequest("Could not add developer");
                }
            });
        }

        [HttpGet]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> GetDeveloper(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.GetDeveloperById(id);
                if (developer != null)
                {
                    _logger.LogInformation("Developer retrieved successfully");
                    return Ok(developer);
                }
                else
                {
                    _logger.LogError("Developer could not be retrieved");
                    return BadRequest("Could not get developer!");
                }
            });
        }

        [HttpGet]
        [Route("api/developers")]
        public async Task<IActionResult> GetDevelopers()
        {
            return await HandleExceptionAsync(async () =>
            {
                var developers = await _developersService.GetDevelopers();
                if (developers != null)
                {
                    _logger.LogInformation("Developers retrieved successfully");
                    return Ok(developers);
                }
                else
                {
                    _logger.LogError("Developers could not be retrieved");
                    return BadRequest("Could not get developers!");
                }
            });
        }


        [HttpPut]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> UpdateDeveloper(int id, [FromBody] Developer model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.UpdateDeveloper(model);

                if (developer != null)
                {
                    _logger.LogInformation("Developer updated successfully");
                    return Ok(developer);
                }
                else
                {
                    _logger.LogError("Developer could not be updated");
                    return NotFound();
                }
            });
        }

        [HttpDelete]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> DeleteDeveloper(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.GetDeveloperById(id);
                var deleted = await _developersService.DeleteDeveloper(developer);

                if (deleted.Id != 0)
                {
                    _logger.LogInformation("Developer deleted successfully");
                    return Ok(deleted);
                }
                else
                {
                    _logger.LogError("Developer could not be deleted");
                    return NotFound();
                }
            });
        }

        [HttpPost]
        [Route("api/developers/login")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var developer = await _developersService.GetDeveloperByEmail(request.Email);
            if (developer == null)
            {
                return BadRequest("Bad credentials");
            }
            if (developer.Password!=request.Password)
            {
                return BadRequest("Bad credentials");
            }

            var accessToken = _tokenService.CreateToken(developer);
            return Ok(new AuthenticationResponse
            {
                Username = developer.UserName,
                Email = developer.Email,
                Token = accessToken,
            });
        }
    }
}
