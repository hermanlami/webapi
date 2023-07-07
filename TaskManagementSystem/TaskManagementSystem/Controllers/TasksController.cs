using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ITasksService _tasksService;
        private readonly ILogger<TasksController> _logger;
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
                if (task.Id > 0)
                {
                    _logger.LogInformation("Task added successfully");
                    return Ok(task);
                }
                else
                {
                    _logger.LogError("Task could not be added");
                    return BadRequest("Failed to add task!");
                }

            });
        }

        [HttpGet]
        [Route("api/tasks")]
        public async Task<IActionResult> GetTasks()
        {
            return await HandleExceptionAsync(async () =>
            {
                var tasks = await _tasksService.GetTasks();
                _logger.LogInformation("Tasks retrieved successfully");

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

                if (task != null)
                {
                    _logger.LogInformation("Task retrieved successfully");

                    return Ok(task);
                }
                else
                {
                    _logger.LogError("Task could not be retrieved");
                    return NotFound();
                }
            });
        }

        [HttpPut]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] BLL.DTO.Task model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var task = await _tasksService.UpdateTask(model);

                if (task != null)
                {
                    _logger.LogInformation("Task updated successfully");
                    return Ok(task);
                }
                else
                {
                    _logger.LogError("Task could not be updated");
                    return BadRequest("Task could not be updated");
                }
            });
        }

        [HttpDelete]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var task = await _tasksService.GetTaskById(id);
                var deleted = await _tasksService.DeleteTask(task);

                if (deleted.Id != 0)
                {
                    _logger.LogInformation("Task deleted successfully");

                    return Ok();
                }
                else
                {
                    _logger.LogError("Task could not be updated");
                    return BadRequest("Task could not be updated");
                }
            });
        }
    }
}
