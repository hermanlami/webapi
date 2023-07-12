using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("api/tasks")]
        public async Task<IActionResult> AddTask([FromBody] BLL.DTO.Task model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var task = await _tasksService.AddTask(model);

                return Ok(task);

            });
        }

        [HttpGet]
        [Route("api/tasks")]
        public async Task<IActionResult> GetTasks()
        {
            return await HandleExceptionAsync(async () =>
            {
                var tasks = await _tasksService.GetTasks();

                return Ok(tasks);
            }
            );
        }

        [HttpGet]
        [Route("api/tasks/completed")]
        public async Task<IActionResult> GetCompletedTasks()
        {
            return await HandleExceptionAsync(async () =>
            {
                var tasks = await _tasksService.GetCompletedTasks();

                return Ok(tasks);
            }
            );
        }

        [HttpGet]
        [Route("api/tasks/byDeveloperUsername/{username}")]
        public async Task<IActionResult> GetTasksByDevelopersUsername(string username)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tasks = await _tasksService.GetTasksByDevelopersUsername(username);

                return Ok(tasks);
            }
            );
        }

        [HttpGet]
        [Route("api/tasks/byProjectName/{name}")]
        public async Task<IActionResult> GetTasksByProjectName(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tasks = await _tasksService.GetTasksByProjectName(name);

                return Ok(tasks);
            }
            );
        }

        [HttpGet]
        [Route("api/tasks/byTagName/{name}")]
        public async Task<IActionResult> GetTasksByTagName(string name)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tasks = await _tasksService.GetTasksByTagName(name);

                return Ok(tasks);
            }
            );
        }

        [HttpGet]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var task = await _tasksService.GetTasks();

                return Ok(task);

            });
        }

        [HttpPut]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] BLL.DTO.Task model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var task = await _tasksService.UpdateTask(id, model);

                return Ok(task);
            });
        }

        [HttpDelete]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _tasksService.DeleteTask(id);

                return Ok(deleted);

            });
        }
    }
}
