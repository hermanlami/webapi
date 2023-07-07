using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Middlewares;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class ProjectManagersController : BaseController
    {
        private readonly IProjectManagersService _projectManagersService;
        private readonly TokenService _tokenService;
        private readonly ILogger<ProjectManagersController> _logger;

        public ProjectManagersController(IProjectManagersService projectManagersService, TokenService tokenService, ILogger<ProjectManagersController> logger)
        {
            _projectManagersService = projectManagersService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/projectManagers")]
        public async Task<IActionResult> AddProjectManager([FromBody] ProjectManager model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.AddProjectManager(model);
                if (pM.Id > 0)
                {
                    _logger.LogInformation("Project manager added sucessfully");
                    return Ok(pM);
                }
                else
                {
                    _logger.LogError("Could not add project manager");

                    return BadRequest("Could not add project manager");
                }
            });
        }

        [HttpGet]
        [Route("api/projectManagers/{id}")]
        public async Task<IActionResult> GetProjectManager( int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.GetProjectManagerById(id);
                if (pM != null)
                {
                    _logger.LogInformation("Project manager retrieved sucessfully");

                    return Ok(pM);
                }
                else
                {
                    _logger.LogError("Could not get project manager");

                    return BadRequest("Could not get project manager!");
                }
            });
        }

        [HttpGet]
        [Route("api/projectManagers")]
        public async Task<IActionResult> GetProjectManagers()
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.GetProjectManagers();
                if (pM != null)
                {
                    _logger.LogInformation("Project managers retrieved sucessfully");

                    return Ok(pM);
                }
                else
                {
                    _logger.LogError("Could not get project managers");

                    return BadRequest("Could not get project managers!");
                }
            });
        }


        [HttpPut]
        [Route("api/projectManagers/{id}")]
        public async Task<IActionResult> UpdateProjectManager(int id, [FromBody] ProjectManager model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.UpdateProjectManager(model);

                if (pM != null)
                {
                    _logger.LogInformation("Project manager updated sucessfully");

                    return Ok(pM);
                }
                else
                {
                    _logger.LogError("Could not update project manager");

                    return BadRequest("Could not update project manager");
                }
            });
        }

        [HttpDelete]
        [Route("api/projectManagers/{id}")]
        public async Task<IActionResult> DeleteProjectManager(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.GetProjectManagerById(id);
                var deleted = await _projectManagersService.DeleteProjectManager(pM);

                if (deleted.Id != 0)
                {
                    _logger.LogInformation("Project manager deleted sucessfully");

                    return Ok(pM);
                }
                else
                {
                    _logger.LogError("Could not delete project manager");

                    return BadRequest("Could not delete project manager");
                }
            });
        }

        [HttpPost]
        [Route("api/projectManagers/login")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pM = await _projectManagersService.GetProjectManagerByEmail(request.Email);
            if (pM == null)
            {
                return BadRequest("Bad credentials");
            }
            if (pM.Password != request.Password)
            {
                return BadRequest("Bad credentials");
            }

            var accessToken = _tokenService.CreateToken(pM);
            return Ok(new AuthenticationResponse
            {
                Username = pM.UserName,
                Email = pM.Email,
                Token = accessToken,
            });
        }
    }
}
