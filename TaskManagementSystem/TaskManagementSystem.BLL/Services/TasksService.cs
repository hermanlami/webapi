using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
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

        public TasksService(ITasksRepository repository, ITaskTagsService taskTagsService, ITagsService tagsService, IProjectsService projectsService,IMapper mapper)
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
                var dalTask = _mapper.Map<DAL.Entities.Task>(model);
                var addedTask = await _repository.AddTask(dalTask);
                if (addedTask.Id > 0)
                {
                    Log.Information("Task added successfully");

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

                    return _mapper.Map<DTO.Task>(model);
                }
                else
                {
                    Log.Error("Task could not be added");

                }

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
                if (task != null)
                {
                    task.IsDeleted = true;
                    var deletedTask = await _repository.DeleteTask(task);
                    if (deletedTask != null)
                    {
                        Log.Information("Task deleted successfully");

                        return _mapper.Map<DTO.Task>(deletedTask);

                    }
                }
                else
                {
                    Log.Error ("Task could not be deleted");

                }
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
                else
                {
                    Log.Error("Tasks could not be retrieved");

                }
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
                    Log.Information("Task retrieved successfully");

                    return _mapper.Map<DTO.Task>(task);
                }
                else
                {
                    Log.Error("Task could not be added");

                }
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
                else
                {
                    Log.Error("Task could not be retrieved");

                }
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
                var developer= await _developersService.GetDeveloperByUsername(username);
                var task = await _repository.GetTasksByDeveloperId(developer.Id);
                if (task != null)
                {
                    Log.Information("Task retrieved successfully");

                    return _mapper.Map<List<DTO.Task>>(task);
                }
                else
                {
                    Log.Error("Task could not be added");

                }
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
                var task = await _repository.GetTasksByProjectId(project.Id);
                if (task != null)
                {
                    Log.Information("Task retrieved successfully");

                    return _mapper.Map<List<DTO.Task>>(task);
                }
                else
                {
                    Log.Error("Task could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<DTO.Task> UpdateTask(int id, DTO.Task model)
        {
            try
            {
                var task = await _repository.GetTaskById(id);
                if (task != null)
                {
                    model.Id = id;
                    var updated = await _repository.UpdateTask(_mapper.Map<DAL.Entities.Task>(model));
                    if (updated != null)
                    {
                        Log.Information("Task updated successfully");

                        return _mapper.Map<DTO.Task>(updated);
                    }
                    else
                    {
                        Log.Error("Task could not be updated");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }
    }
}
