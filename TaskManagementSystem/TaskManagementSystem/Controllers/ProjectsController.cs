using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "Developer"} })]

    public class ProjectsController : BaseController
    {
        private readonly IProjectsService _projectsService;
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("api/projects")]
        public async Task<IActionResult> AddProject([FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.AddProject(model);
                return Ok(project);

            });
        }

        [HttpGet]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.GetProjectById(id);

                return Ok(project);

            });
        }

        [HttpGet]
        [Route("api/projects")]
        public async Task<IActionResult> GetProjects()
        {
            return await HandleExceptionAsync(async () =>
            {
                var projects = await _projectsService.GetProjects();
                return Ok(projects);

            });
        }


        [HttpPut]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.UpdateProject(id, model);
                return Ok(project);

            });
        }

        [HttpDelete]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _projectsService.DeleteProject(id);
                return Ok(deleted);
            });
        }
    }
}
