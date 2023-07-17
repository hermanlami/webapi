using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin" } })]

    public class ProjectManagersController : BaseController
    {
        private readonly IProjectManagersService _projectManagersService;
        private readonly ITokensService _tokensService;

        public ProjectManagersController(IProjectManagersService projectManagersService, ITokensService tokensService)
        {
            _projectManagersService = projectManagersService;
            _tokensService = tokensService;
        }

        [HttpPost]
        [Route("api/projectManagers")]
        public async Task<IActionResult> AddProjectManager([FromBody] ProjectManager model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.AddProjectManager(model);
                return Ok(pM);

            });
        }

        [HttpGet]
        [Route("api/projectManagers/{id}")]
        public async Task<IActionResult> GetProjectManager(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.GetProjectManagerById(id);
                return Ok(pM);
            });
        }

        [HttpGet]
        [Route("api/projectManagers")]
        public async Task<IActionResult> GetProjectManagers()
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.GetProjectManagers();
                return Ok(pM);

            });
        }


        [HttpPut]
        [Route("api/projectManagers/{id}")]
        public async Task<IActionResult> UpdateProjectManager(int id, [FromBody] ProjectManager model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var pM = await _projectManagersService.UpdateProjectManager(id, model);
                return Ok(pM);
            });
        }

        [HttpDelete]
        [Route("api/projectManagers/{id}")]
        public async Task<IActionResult> DeleteProjectManager(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _projectManagersService.DeleteProjectManager(id);
                return Ok(deleted);

            });
        }
    }
}
