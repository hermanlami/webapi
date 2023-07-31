using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Drawing;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common.CustomExceptions;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    public class TasksService : ITasksService
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
        /// <summary>
        /// Krijon nje task te ri.
        /// </summary>
        /// <param name="model">Modeli ne baze te te cilit do te krijohet task-u i ri.</param>
        /// <returns>Task-un e krijuar.</returns>
        public async Task<DTO.Task> AddTask(DTO.Task model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (await _repository.GetTaskByName(model.Name) != null)
                {
                    Log.Error($"Task {model.Name} already exists");
                    throw new DuplicateInputException($"Task {model.Name} already exists");
                }

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
        /// <summary>
        /// Fshin nje task.
        /// </summary>
        /// <param name="id">Id qe sherben per identifikimin e task-ut.</param>
        /// <returns>Task-un e fshire.</returns>
        public async Task<DTO.Task> DeleteTask(string name)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskByName(name);
                if (task == null)
                {
                    Log.Error("Task not found");
                    throw new NotFoundException("Task not found");

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
        /// <summary>
        /// Merr te gjitha task-et qe rezutojne te perfunduara.
        /// </summary>
        /// <returns>Kthen listen e task-eve te perfunduara.</returns>
        public async Task<List<DTO.Task>> GetCompletedTasks(string userRole, int userId)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tasks = await _repository.GetCompletedTasks();
                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    return userRole == PersonType.ProjectManager.ToString()? _mapper.Map<List<DTO.Task>>(tasks.Where(x => x.Project.ProjectManagerId == userId)) : _mapper.Map<List<DTO.Task>>(tasks);
                }

                Log.Error("Tasks could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            });
        }
        /// <summary>
        /// Merr nje task ne baze te emrit te tij.
        /// </summary>
        /// <param name="name">Emri qe identifikon task-un qe duhet marre.</param>
        /// <returns>Task-un perkates.</returns>
        public async Task<DTO.Task> GetTaskByName(string name, string userRole="", int userId=0)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskByName(name, 0,userId, userRole);
                if (task != null)
                {
                    Log.Information($"Task {task.Name} retrieved successfully");
                    
                        return _mapper.Map<DTO.Task>(task);
                }

                Log.Error("Task not found");
                throw new NotFoundException("Task not found");

            });
        }

        /// <summary>
        /// Merr te gjitha task-et.
        /// </summary>
        /// <param name="userId">Id qe identikifon perdoruesin e loguar.</param>
        /// <param name="userRole">Tregon rolin e perdoruesit te loguar(Admin, Project Manager, Developer).</param>
        /// <returns>Nese perdoruesi i loguar eshte developer kthen task-et qe i perkasin ketij develoepr-i,
        /// perndryshe kthen te gjitha task-et.<returns>


        public async Task<List<DTO.Task>> GetTasks(string userRole, int userId)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tasks = userRole == PersonType.Developer.ToString() ? await _repository.GetTasks(userId) : await _repository.GetTasks();

                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    return userRole == PersonType.ProjectManager.ToString() ? _mapper.Map<List<DTO.Task>>(tasks.Where(x => x.Project.ProjectManagerId == userId)) : _mapper.Map<List<DTO.Task>>(tasks);

                }

                Log.Error("Task could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            });
        }
        /// <summary>
        /// Merr te gjitha task-et qe i perkasin developer-it me nje username te caktuar.
        /// </summary>
        /// <param name="username">Username qe identifikon developer-in.<param>
        /// <returns>Listen e te gjitha taske-ve te developer-it perkates.</returns>
        public async Task<List<DTO.Task>> GetTasksByDevelopersUsername(string username, string userRole, int userId)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var developer = await _peoplesService.GetPersonByUsername(username);
                if (developer == null)
                {
                    Log.Error($"User with username {username} does not exist");
                    throw new NotFoundException("User does not exist");

                }
                var tasks = await _repository.GetTasksByDeveloperId(developer.Id);
                if (tasks != null)
                {
                    Log.Information($"Tasks of developer with username {username} retrieved successfully");
                    return userRole == PersonType.ProjectManager.ToString() ? _mapper.Map<List<DTO.Task>>(tasks.Where(x => x.Project.ProjectManagerId == userId)) : _mapper.Map<List<DTO.Task>>(tasks);
                }

                Log.Error($"Tasks could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");
            });
        }
        /// <summary>
        /// Merr task-et qe i perkasin nje projekti me nje emer te caktuar.
        /// </summary>
        /// <param name="name">Emri qe sherben per te identfikuar projektin.</param>
        /// <returns>Listen e task-eve te projektit perkates.</returns>
        public async Task<List<DTO.Task>> GetTasksByProjectName(string name, string userRole, int userId)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var project = await _projectsService.GetProjectByName(name);
                if (project == null)
                {
                    Log.Error($"Project {name} not found");

                    throw new NotFoundException("Project not found");
                }
                var tasks = userRole == PersonType.Developer.ToString() ? await _repository.GetTasksByProjectId(project.Id, userId) : await _repository.GetTasksByProjectId(project.Id);
                if (tasks != null)
                {
                    Log.Information($"Task of project {name} retrieved successfully");
                    return userRole == PersonType.ProjectManager.ToString() ? _mapper.Map<List<DTO.Task>>(tasks.Where(x => x.Project.ProjectManagerId == userId)) : _mapper.Map<List<DTO.Task>>(tasks);

                }

                Log.Error("Task could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            });
        }
        /// <summary>
        /// Merr task-et qe i permbajne tag-un me nje emer te caktuar.
        /// </summary>
        /// <param name="name">Emri qe sherben per te identfikuar tag-un.</param>
        /// <returns>Listen e task-eve qe permbajne tag-un perkates.</returns>
        public async Task<List<DTO.Task>> GetTasksByTagName(string name, string userRole, int userId)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.GetTagByName(name);
                if (tag == null)
                {
                    Log.Error($"Tag {name} not found");
                    throw new NotFoundException("Tag not found");
                }
                var taskTags = await _taskTagsService.GetTaskTagByTagId(tag.Id);
                if (taskTags == null)
                {
                    Log.Information("Task tags could not be retrieved");
                    return new List<DTO.Task>();
                }

                var tasks = await TaskByTags(userRole, userId, taskTags);

                return _mapper.Map<List<DTO.Task>>(tasks);

            });
        }
        /// <summary>
        /// Njofton developer per task-et afati i perfundimit te te cilave po afrohet.
        /// </summary>
        public async System.Threading.Tasks.Task NotifyForTasksCloseToDeadline()
        {
            await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tasks = await _repository.GetTasksCloseToDeadline();
                if (tasks == null)
                {
                    throw new CustomException("Tasks could not be retrieved");
                }
                Log.Information("Tasks retrieved successfully");
                foreach (var task in tasks)
                {
                    var developer = await _developersService.GetDeveloperById(task.DeveloperId);
                    var daysLeft = (task.EndDate - DateTime.Now).TotalDays;
                    Mail.DeadlineNotification(developer.Email, task.Name, daysLeft);
                }

            });
        }
        /// <summary>
        /// E ben nje task te perfunduar.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifkuar task-un.</param>
        /// <returns>Task-un perkates.</returns>
        public async Task<DTO.Task> SetTaskStatus(string name, string userRole, int userId, string response)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskByName(name, 0, userId, userRole);
                if (task == null)
                {
                    Log.Error("Task not found");
                    throw new NotFoundException("Task not found");
                }

                var developer = await _developersService.GetDeveloperById(task.DeveloperId);

                if (userRole == PersonType.Developer.ToString())
                {
                    task.PendingStatus = true;
                }
                else
                {
                    if (response == "accept")
                    {
                        task.FinalStatus = true;
                    }
                    if (response == "reject")
                    {
                        task.PendingStatus = false;
                        task.FinalStatus = false;
                    }
                }
                var updated = await _repository.UpdateTask(_mapper.Map<DAL.Entities.Task>(task));
                if (updated != null)
                {
                    if (task.PendingStatus && userRole!=PersonType.Developer.ToString())
                    {
                        Mail.TaskCompletionResponse(developer.Email, response);
                    }
                    Log.Information("Task marked as completed");
                    return _mapper.Map<DTO.Task>(updated);
                }
                Log.Error("Could not mark task as completed");
                throw new CustomException("Could not mark task as completed");
            });
        }

        /// <summary>
        /// Perditeson te dhenat e nje task-u.
        /// </summary>
        /// <param name="name">Emri qe sherben per te identifkuar task-un.</param>
        /// <param name="model">Modeli ne baze te te cilit do te behet perditesimi i te dhenave.</param>
        /// <returns>Task-un e perditesuar.</returns>
        public async Task<DTO.Task> UpdateTask(string name, DTO.Task model, int userId, string userRole)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {

                if (await _repository.GetTaskByName(model.Name, model.Id) != null)
                {
                    Log.Error($"Task {model.Name} already exists");
                    throw new DuplicateInputException($"Task {model.Name} already exists");
                }

                var task = await _repository.GetTaskByName(name, 0, userId,userRole);
                if (task == null)
                {
                    Log.Error("Task not found");
                    throw new NotFoundException("Task not found");
                }

                await _taskTagsService.DeleteTaskTags(task.Id);

                model.Id= task.Id;

                var updated = await _repository.UpdateTask(_mapper.Map(model, task));

                if (updated != null)
                {
                    var tagNames = model.Tags.Split(',');
                    foreach (var tagName in tagNames)
                    {
                        var tag = await _tagsService.GetTagByName(tagName);
                        await _taskTagsService.AddTaskTag(new TaskTag()
                        {
                            TagId = tag.Id,
                            TaskId = updated.Id,
                        });
                    }
                    Log.Information("Task updated successfully");
                    return _mapper.Map<DTO.Task>(updated);
                }

                Log.Error("Task could not be updated");
                throw new CustomException("Task could not be updated");

            });
        }
        /// <summary>
        /// Merr te githe task-et qe permbajne nje tag te caktuar nese perdoruesi eshte admin ose project manager. Nese perdoruesi
        /// eshte developer kthen task-et qe permbajne tag-un perkates dhe qe i perkasin atij developer.
        /// </summary>
        /// <param name="userRole">Roli i perdoruesit te loguar.</param>
        /// <param name="userId">Id e perdoruesit te loguar.</param>
        /// <param name="taskTags">Lista e tasktag-s qe permbajne Id e tag-ut sipas te cilit do te filtrohen task-et.</param>
        /// <returns>Listen me task-et perkates.</returns>
        private async Task<List<DAL.Entities.Task>> TaskByTags(string userRole, int userId, List<TaskTag> taskTags)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tasks = new List<DAL.Entities.Task>();
                if (userRole == PersonType.Developer.ToString())
                {
                    foreach (var taskTag in taskTags)
                    {
                        var task = await _repository.GetTaskById(taskTag.TaskId, userId);
                        if (task != null)
                        {
                            tasks.Add(task);
                        }
                    }
                    return tasks;
                }
                else
                {
                    foreach (var taskTag in taskTags)
                    {
                        var task = await _repository.GetTaskById(taskTag.TaskId);
                        if (task != null)
                        {
                            if ((userRole == PersonType.ProjectManager.ToString() && task.Project.ProjectManagerId == userId)||userRole==PersonType.Admin.ToString())
                            {
                                tasks.Add(task);
                            }
                        }
                    }
                    return tasks;
                }
            });
        }
    }
}
