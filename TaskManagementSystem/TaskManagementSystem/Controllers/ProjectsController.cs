using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.Middlewares;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    //[TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "Developer"} })]

    public class ProjectsController : BaseController
    {
        private readonly IProjectsService _projectsService;
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }
        /// <summary>
        /// Krijon nje projekt.
        /// </summary>
        /// <param name="model">Modeli ne baze te te cilit do te behet krijimi.</param>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/projects")]
        public async Task<IActionResult> AddProject([FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var project = await _projectsService.AddProject(model);
                return Ok(project);

            });
        }
        /// <summary>
        /// Merr nje projekt ne baze te id se tij.
        /// </summary>
        /// <param name="id">Id qe identifikon projektin.</param>
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
        /// <summary>
        /// Merr te gjithe projektet.
        /// </summary>
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

        /// <summary>
        /// Perditeson te dhenat e nje projekti.
        /// </summary>
        /// <param name="id">Id qe identifikon projektin.</param>
        /// <param name="model">Modeli ne baze te te cilit do te behet perditesimi.</param>
        [HttpPut]
        [Route("api/projects/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var project = await _projectsService.UpdateProject(id, model);
                return Ok(project);

            });
        }
        /// <summary>
        /// Fshin nje projekt.
        /// </summary>
        /// <param name="id">Id qe identifikon projektin.</param>
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
