using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class ProjectsController : BaseController
    {
        private readonly IProjectsService _projectsService;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(IProjectsService projectsService, ILogger<ProjectsController> logger)
        {
            _projectsService = projectsService;
            _logger = logger;
        }
        [HttpPost]
        [Route("api/projects")]
        public async Task<IActionResult> AddProject([FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var project=await _projectsService.AddProject(model);
                if(project.Id>0)
                {
                    _logger.LogInformation("Project added successfully");
                    return Ok(project);
                }
                else
                {
                    _logger.LogError("Project could not be added");
                    return BadRequest("Could not add project");
                }
            });
        }

        [HttpGet]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> GetProject(int id)  
        {
            return await HandleExceptionAsync(async () =>
            {
                var project= await _projectsService.GetProjectById(id);
                if(project!=null)
                {
                    _logger.LogInformation("Project retrieved successfully");

                    return Ok(project);
                }
                else
                {
                    _logger.LogError("Project could not be retrieved");

                    return BadRequest("Could not get project!"); 
                }
            });
        }

        [HttpGet]
        [Route("api/projects")]
        public async Task<IActionResult> GetProjects()
        {
            return await HandleExceptionAsync(async () =>
            {
                var projects = await _projectsService.GetProjects();
                if (projects != null)
                {
                    _logger.LogInformation("Projects retrieved successfully");

                    return Ok(projects);
                }
                else
                {
                    _logger.LogError("Projects could not be added");

                    return BadRequest("Could not get projects!");
                }
            });
        }


        [HttpPut]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.UpdateProject(model);

                if (project != null)
                {
                    _logger.LogInformation("Project updated successfully");
                    return Ok(project);
                }
                else
                {
                    _logger.LogError("Project could not be updated");

                    return NotFound();
                }
            });
        }

        [HttpDelete]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.GetProjectById(id);
                var deleted = await _projectsService.DeleteProject(project);

                if (deleted.Id != 0)
                {
                    _logger.LogInformation("Project deleted successfully");
                    return Ok(project);
                }
                else
                {
                    _logger.LogError("Project could not be deleted");

                    return NotFound();
                }
            });
        }
    }
}
