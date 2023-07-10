using AutoMapper;
using Microsoft.Extensions.Logging;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TaskTagsService : ITaskTagsService
    {
        private readonly ITaskTagsRepository _repository;
        private readonly ILogger<TaskTagsService> _logger;
        private readonly IMapper _mapper;
        public TaskTagsService(ITaskTagsRepository repository, ILogger<TaskTagsService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<TaskTag> AddTaskTag(TaskTag model)
        {
            try
            {
                var dalTaskTag = _mapper.Map<DAL.Entities.TaskTag>(model);
                var addedTaskTag = await _repository.AddTaskTag(dalTaskTag);
                if (addedTaskTag.Id > 0)
                {
                    _logger.LogInformation("Task tag added successfully");
                    return _mapper.Map<TaskTag>(addedTaskTag);
                }
                else
                {
                    _logger.LogError("Task tag could not be added");
                }

            }
            catch (Exception ex)
            {

            }
            return new DTO.TaskTag();
        }

        public async Task<TaskTag> DeleteTaskTag(TaskTag model)
        {
            try
            {
                var taskTag = await _repository.GetTaskTagById(model.Id);
                if (taskTag != null)
                {
                    var deletedTaskTag = await _repository.DeleteTaskTag(taskTag);
                    if (deletedTaskTag != null)
                    {
                        _logger.LogInformation("Task tag deleted successfully");

                        return _mapper.Map<TaskTag>(deletedTaskTag);


                    }
                    else
                    {
                        _logger.LogError("Task tag could not be deleted");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.TaskTag();
        }

        public async Task<TaskTag> GetTaskTagById(int id)
        {
            try
            {
                var task = await _repository.GetTaskTagById(id);
                if (task != null)
                {
                    _logger.LogInformation("Task tag retrieved successfully");

                    return _mapper.Map<TaskTag>(task);
                }
                else
                {
                    _logger.LogError("Task tag could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new TaskTag();
        }

        public async Task<List<TaskTag>> GetTaskTags()
        {
            try
            {
                var taskTags = await _repository.GetTaskTags();
                if (taskTags != null)
                {
                    _logger.LogInformation("Task tags retrieved successfully");

                    return _mapper.Map<List<TaskTag>>(taskTags);
                }
                else
                {
                    _logger.LogError("Task tags could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<TaskTag>();
        }

        public async Task<TaskTag> UpdateTaskTag(TaskTag model)
        {
            try
            {
                var taskTag = await _repository.GetTaskTagById(model.Id);
                if (taskTag != null)
                {
                    taskTag.TagId = model.TagId;
                    taskTag.TaskId = model.TaskId;
                    var updated = await _repository.UpdateTaskTag(taskTag);
                    if (updated != null)
                    {
                        _logger.LogInformation("Task tag updated successfully");

                        return _mapper.Map<TaskTag>(updated);
                    }
                    else
                    {
                        _logger.LogError("Task tag could not be updated");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.TaskTag();
        }
    }
}
