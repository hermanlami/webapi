using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ITasksService _tasksService;
        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }
        /// <summary>
        /// Krijon nje task te ri.
        /// </summary>
        /// <param name="model">Modeli ne baze te te cilit do te krijohet task-u.</param>
        [HttpPost]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]
        [Route("api/tasks")]
        public async Task<IActionResult> AddTask([FromBody] BLL.DTO.Task model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var task = await _tasksService.AddTask(model);
                return Ok(task);

            });
        }
        /// <summary>
        /// Merr te gjithe task-et qe i takojne nje developer-i, nese eshte i tille ai qe e ben kerkesen, 
        /// perndyshe merr te gjithe task-et pavaresisht se kujt i perkasin.
        /// </summary>
        [HttpGet]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager", "Developer" } })]
        [Route("api/tasks")]
        public async Task<IActionResult> GetTasks()
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var tasks = await _tasksService.GetTasks(userRole, userId);
                return Ok(tasks);
            }
            );
        }
        /// <summary>
        /// Merr task-et e perfunduara.
        /// </summary>
        [HttpGet]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]

        [Route("api/tasks/completed")]
        public async Task<IActionResult> GetCompletedTasks()
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var tasks = await _tasksService.GetCompletedTasks(userRole, userId);
                return Ok(tasks);
            }
            );
        }
        /// <summary>
        /// Merr tasket qe i perkasin nje developer-i me nje username te caktuar
        /// </summary>
        /// <param name="username">Username i developer.</param>
        [HttpGet]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]

        [Route("api/tasks/byDeveloperUsername/{username}")]
        public async Task<IActionResult> GetTasksByDevelopersUsername(string username)
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var tasks = await _tasksService.GetTasksByDevelopersUsername(username, userRole, userId);
                return Ok(tasks);
            }
            );
        }
        /// <summary>
        /// Merr task-et sipas emrit te projektit qe i perkasin.
        /// </summary>
        /// <param name="name">Emri i projektit</param>
        [HttpGet]
        [Route("api/tasks/byProjectName/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager", "Developer" } })]
        public async Task<IActionResult> GetTasksByProjectName(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var tasks = await _tasksService.GetTasksByProjectName(name, userRole, userId);
                return Ok(tasks);
            }
            );
        }
        /// <summary>
        /// Merr tasket sipas emrit te tag-ut.
        /// </summary>
        /// <param name="name">Emri i tag-ut</param>
        [HttpGet]
        [Route("api/tasks/bytagname/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager", "Developer" } })]
        public async Task<IActionResult> GetTasksByTagName(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var tasks = await _tasksService.GetTasksByTagName(name, userRole, userId);
                return Ok(tasks);
            }
            );
        }
        /// <summary>
        /// Merr task ne baze te id se tij.
        /// </summary>
        /// <param name="name">Emri qe identifikon task-un.</param>
        [HttpGet]
        [Route("api/tasks/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager", "Developer" } })]
        public async Task<IActionResult> GetTask(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var task = await _tasksService.GetTaskByName(name, userRole, userId);
                return Ok(task);

            });
        }
        /// <summary>
        /// Deklaron task-un si te perfunduar.
        /// </summary>
        /// <param name="name">Emri qe identifikon task-un.</param>
        [HttpPut]
        [Route("api/tasks/complete/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager", "Developer" } })]
        public async Task<IActionResult> SetTaskStatus(string name, string response)
        {
            return await HandleExceptionAsync(async () =>
            {
                var userRole = UserRole(out int userId);
                var task = await _tasksService.SetTaskStatus(name, userRole, userId, response);
                return Ok(task);
            });
        }
        /// <summary>
        /// Perditeson te dhenat e task-ut.
        /// </summary>
        /// <param name="name">Id qe identifikon task-un.</param>
        /// <param name="model">Modeli ne baze te te cilit behet perditesimi.</param>
        [HttpPut]
        [Route("api/tasks/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]

        public async Task<IActionResult> UpdateTask(string name, [FromBody] BLL.DTO.Task model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userRole = UserRole(out int userId);
                var task = await _tasksService.UpdateTask(name, model, userId, userRole);
                return Ok(task);
            });
        }
        /// <summary>
        /// Fshin nje task.
        /// </summary>
        /// <param name="name">Emri qe identifikon task-un.</param>
        [HttpDelete]
        [Route("api/tasks/{name}")]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager"} })]

        public async Task<IActionResult> DeleteTask(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _tasksService.DeleteTask(name);
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
