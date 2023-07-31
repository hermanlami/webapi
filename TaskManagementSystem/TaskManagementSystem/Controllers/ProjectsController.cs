using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.Middlewares;

namespace TaskManagementSystem.Controllers
{
    [ApiController]

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
        [Route("api/projects")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin"} })]

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
        /// <param name="name">Emri qe identifikon projektin.</param>
        [HttpGet]
        [Route("api/projects/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]
        public async Task<IActionResult> GetProject(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var project = await _projectsService.GetProjectByName(name, userRole, userId);
                return Ok(project);

            });
        }
        /// <summary>
        /// Merr te gjithe projektet.
        /// </summary>
        [HttpGet]
        [Route("api/projects")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]
        public async Task<IActionResult> GetProjects()
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var projects = await _projectsService.GetProjects(userId, userRole);
                return Ok(projects);

            });
        }

        /// <summary>
        /// Perditeson te dhenat e nje projekti.
        /// </summary>
        /// <param name="name">Emri qe identifikon projektin.</param>
        /// <param name="model">Modeli ne baze te te cilit do te behet perditesimi.</param>
        [HttpPut]
        [Route("api/projects/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin" } })]
        public async Task<IActionResult> UpdateProject(string name, [FromBody] Project model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var project = await _projectsService.UpdateProject(name, model);
                return Ok(project);

            });
        }
        /// <summary>
        /// Fshin nje projekt.
        /// </summary>
        /// <param name="name">Emri qe identifikon projektin.</param>
        [HttpDelete]
        [Route("api/projects/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin" } })]
        public async Task<IActionResult> DeleteProject(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _projectsService.DeleteProject(name);
                return Ok(deleted);
            });
        }

        private string UserRole(out int userId)
        {
            Int32.TryParse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out userId);
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }
    }
}
