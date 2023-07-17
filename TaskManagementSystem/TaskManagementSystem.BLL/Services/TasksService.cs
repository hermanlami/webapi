using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TasksService : ITasksService
    {
        private readonly ITasksRepository _repository;
        private readonly ITaskTagsService _taskTagsService;
        private readonly ITagsService _tagsService;
        private readonly IDevelopersService _developersService;
        private readonly IProjectsService _projectsService;
        private readonly IAuthenticationsService _peoplesService;

        private readonly IMapper _mapper;

        public TasksService(ITasksRepository repository, ITaskTagsService taskTagsService, ITagsService tagsService, IProjectsService projectsService, IMapper mapper, IAuthenticationsService peoplesService, IDevelopersService developersService)
        {
            _repository = repository;
            _taskTagsService = taskTagsService;
            _tagsService = tagsService;
            _projectsService = projectsService;
            _developersService = developersService;
            _mapper = mapper;
            _peoplesService = peoplesService;
        }
        public async Task<DTO.Task> AddTask(DTO.Task model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var addedTask = await _repository.AddTask(_mapper.Map<DAL.Entities.Task>(model));
                if (addedTask.Id == 0)
                {
                    Log.Error("Task could not be added");
                    throw new CustomException("Task could not be added");
                }

                var tagNames = model.Tags.Split(',');
                foreach (var tagName in tagNames)
                {
                    var tag = await _tagsService.GetTagByName(tagName);
                    await _taskTagsService.AddTaskTag(new TaskTag()
                    {
                        TagId = tag.Id,
                        TaskId = addedTask.Id,
                    });
                }
                Log.Information($"Task {addedTask.Name} added successfully");
                return _mapper.Map<DTO.Task>(model);

            });
        }

        public async Task<DTO.Task> DeleteTask(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskById(id);
                if (task == null)
                {
                    Log.Error("Task not found");
                    throw new CustomException("Task not found");

                }

                task.IsDeleted = true;
             
                var deletedTask = await _repository.DeleteTask(task);
                if (deletedTask != null)
                {
                    Log.Information($"Task {deletedTask.Name} deleted successfully");
                    return _mapper.Map<DTO.Task>(deletedTask);
                }
                Log.Error($"Task {task.Name} could not be deleted");
                throw new CustomException("Task could not be deleted");


            });
        }

        public async Task<List<DTO.Task>> GetCompletedTasks()
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tasks = await _repository.GetCompletedTasks();
                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(tasks);
                }

                Log.Error("Tasks could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            });
        }

        public async Task<DTO.Task> GetTaskById(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskById(id);
                if (task != null)
                {
                    Log.Information($"Task {task.Name} retrieved successfully");
                    return _mapper.Map<DTO.Task>(task);
                }

                Log.Error("Task not found");
                throw new CustomException("Task not found");

            });
        }

        public async Task<List<DTO.Task>> GetTasks()
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tasks = await _repository.GetTasks();
                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(tasks);
                }

                Log.Error("Task could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            });
        }

        public async Task<List<DTO.Task>> GetTasksByDevelopersUsername(string username)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var developer = await _peoplesService.GetPersonByUsername(username);
                if (developer == null)
                {
                    Log.Error($"User with username {username} does not exist");
                    throw new CustomException("User does not exist");

                }
                var task = await _repository.GetTasksByDeveloperId(developer.Id);
                if (task != null)
                {
                    Log.Information($"Tasks of developer with username {username} retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(task);
                }

                Log.Error($"Tasks could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");
            });
        }

        public async Task<List<DTO.Task>> GetTasksByProjectName(string name)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.GetProjectByName(name);
                if (project == null)
                {
                    Log.Error($"Project {name} not found");

                    throw new CustomException("Project not found");
                }
                var task = await _repository.GetTasksByProjectId(project.Id);
                if (task != null)
                {
                    Log.Information($"Task of project {name} retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(task);
                }

                Log.Error("Task could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            });
        }

        public async Task<List<DTO.Task>> GetTasksByTagName(string name)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.GetTagByName(name);
                if (tag == null)
                {
                    Log.Error($"Tag {name} not found");
                    throw new CustomException("Tag not found");
                }
                var taskTags = await _taskTagsService.GetTaskTagByTagId(tag.Id);
                if (taskTags == null)
                {
                    Log.Information("Task tags could not be retrieved");
                    return new List<DTO.Task>();
                }

                var tasks = new List<DAL.Entities.Task>();
                foreach (TaskTag taskTag in taskTags)
                {
                    tasks.Add(await _repository.GetTaskById(taskTag.TaskId));
                }

                return _mapper.Map<List<DTO.Task>>(tasks);

            });
        }

        public async System.Threading.Tasks.Task NotifyForTasksCloseToDeadline()
        {
            try { 
                var tasks = await _repository.GetTasksCloseToDeadline();
                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    foreach (var task in tasks)
                    {
                        var developer = await _developersService.GetDeveloperById(task.DeveloperId);
                        var daysLeft = (task.EndDate - DateTime.Now).TotalDays;
                        Mail.DeadlineNotification(developer.Email, daysLeft);
                    }
                }
                throw new CustomException("Tasks could not be retrieved"); 
            }catch(CustomException ex)
            {

            }
        }

        public async Task<DTO.Task> MarkTaskAsCompleted(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskById(id);
                if (task == null)
                {
                    Log.Error("Task not found");
                    throw new CustomException("Task not found");
                }

                task.Status = true;
                var updated = await _repository.UpdateTask(task);
                if (updated != null)
                {
                    Log.Information("Task marked as completed");
                    return _mapper.Map<DTO.Task>(updated);
                }
                Log.Error("Could not mark task as completed");
                throw new CustomException("Could not mark task as completed");
            });
        }

        public async Task<DTO.Task> UpdateTask(int id, DTO.Task model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskById(id);
                if (task == null)
                {
                    Log.Error("Task not found");
                    throw new CustomException("Task not found");

                }

                model.Id = id;
                var updated = await _repository.UpdateTask(_mapper.Map<DAL.Entities.Task>(model));
                if (updated != null)
                {
                    Log.Information("Task updated successfully");
                    return _mapper.Map<DTO.Task>(updated);
                }

                Log.Error("Task could not be updated");
                throw new CustomException("Task could not be updated");

            });
        }
    }
}
