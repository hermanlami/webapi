using Microsoft.Extensions.Logging;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TaskTagsService:ITaskTagsService
    {
        private readonly ITaskTagsRepository _repository;
        private readonly ILogger<TaskTagsService> _logger;
        public TaskTagsService(ITaskTagsRepository repository,ILogger<TaskTagsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<TaskTag> AddTaskTag(TaskTag model)
        {
            try
            {
                var dalTaskTag = new DAL.Entities.TaskTag()
                {
                    TagId = model.Id,
                    TaskId=model.TagId
                };
                var addedTaskTag = await _repository.AddTaskTag(dalTaskTag);
                model.Id = addedTaskTag.Id;
                if (addedTaskTag.Id > 0)
                {
                    _logger.LogInformation("Task tag added successfully");
                    return model;
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
                var taskTag= await _repository.GetTaskTagById(model.Id);
                if (taskTag != null)
                {
                    var deletedTaskTag = await _repository.DeleteTaskTag(taskTag);
                    if (deletedTaskTag != null)
                    {
                        _logger.LogInformation("Task tag deleted successfully");

                        return new DTO.TaskTag()
                        {
                           TagId=deletedTaskTag.Id,
                           TaskId=deletedTaskTag.TaskId
                        };

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

                    return new DTO.TaskTag()
                    {
                        Id = task.Id,
                        TaskId = task.TaskId,
                        TagId = task.TagId,
                        
                    };
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
                var dtoTaskTags = new List<TaskTag>();
                if (taskTags != null)
                {
                    _logger.LogInformation("Task tags retrieved successfully");

                    taskTags.ForEach(x => dtoTaskTags.Add(new DTO.TaskTag
                    {
                        Id = x.Id,
                        TagId = x.TagId,
                        TaskId = x.TaskId,
                    }));
                    return dtoTaskTags;
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

                        return new DTO.TaskTag()
                        {
                            TaskId = updated.TaskId,
                            TagId = updated.TagId,
                        };
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
