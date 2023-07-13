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

        private readonly IMapper _mapper;

        public TasksService(ITasksRepository repository, ITaskTagsService taskTagsService, ITagsService tagsService, IProjectsService projectsService, IMapper mapper)
        {
            _repository = repository;
            _taskTagsService = taskTagsService;
            _tagsService = tagsService;
            _projectsService = projectsService;
            _mapper = mapper;
        }
        public async Task<DTO.Task> AddTask(DTO.Task model)
        {
            try
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

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }

        public async Task<DTO.Task> DeleteTask(int id)
        {
            try
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


            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }

        public async Task<List<DTO.Task>> GetCompletedTasks()
        {
            try
            {
                var tasks = await _repository.GetCompletedTasks();
                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(tasks);
                }

                Log.Error("Tasks could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<DTO.Task> GetTaskById(int id)
        {
            try
            {
                var task = await _repository.GetTaskById(id);
                if (task != null)
                {
                    Log.Information($"Task {task.Name} retrieved successfully");
                    return _mapper.Map<DTO.Task>(task);
                }

                Log.Error("Task not found");
                throw new CustomException("Task not found");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }

        public async Task<List<DTO.Task>> GetTasks()
        {
            try
            {
                var tasks = await _repository.GetTasks();
                if (tasks != null)
                {
                    Log.Information("Tasks retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(tasks);
                }

                Log.Error("Task could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<List<DTO.Task>> GetTasksByDevelopersUsername(string username)
        {
            try
            {
                var developer = await _developersService.GetDeveloperByUsername(username);
                if (developer == null)
                {
                    Log.Error($"Developer with username {username} does not exist");
                    throw new CustomException("Developer does not exist");

                }
                var task = await _repository.GetTasksByDeveloperId(developer.Id);
                if (task != null)
                {
                    Log.Information($"Tasks of developer with username {username} retrieved successfully");
                    return _mapper.Map<List<DTO.Task>>(task);
                }

                Log.Error($"Tasks could not be retrieved");
                throw new CustomException("Tasks could not be retrieved");
            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<List<DTO.Task>> GetTasksByProjectName(string name)
        {
            try
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

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<List<DTO.Task>> GetTasksByTagName(string name)
        {
            try
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

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<DTO.Task> MarkTaskAsCompleted(int id)
        {
            try
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
            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }

        public async Task<DTO.Task> UpdateTask(int id, DTO.Task model)
        {
            try
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

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }
    }
}
