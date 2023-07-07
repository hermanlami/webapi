using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TasksService : ITasksService
    {
        private readonly ITasksRepository _repository;
        private readonly ITaskTagsRepository _taskTagsRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly ILogger<TasksService> _logger;

        public TasksService(ITasksRepository repository, ITaskTagsRepository taskTagsRepository, ITagsRepository tagsRepository, ILogger<TasksService> logger)
        {
            _repository = repository;
            _taskTagsRepository = taskTagsRepository;
            _tagsRepository = tagsRepository;
            _logger = logger;
        }
        public async Task<DTO.Task> AddTask(DTO.Task model)
        {
            try
            {
                var dalTask = new DAL.Entities.Task()
                {
                    Name = model.Name,
                    EndDate = model.EndDate,
                    Description = model.Description,
                    Importance = model.Importance,
                    ProjectId = model.ProjectId,
                };
                var addedTask = await _repository.AddTask(dalTask);
                model.Id = addedTask.Id;
                if (addedTask.Id > 0)
                {
                    _logger.LogInformation("Task added successfully");
                    var tagNames = model.Tags.Split(',');
                    foreach (var tagName in tagNames)
                    {
                        var tag = await _tagsRepository.GetTagByName(tagName);
                        await _taskTagsRepository.AddTaskTag(new DAL.Entities.TaskTag()
                        {
                            TagId = tag.Id,
                            TaskId = addedTask.Id,
                        });
                    }
                    return model;
                }
                else
                {
                    _logger.LogError("Task could not be added");

                }

            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }

        public async Task<DTO.Task> DeleteTask(DTO.Task model)
        {
            try
            {
                var task = await _repository.GetTaskById(model.Id);
                if (task != null)
                {
                    task.IsDeleted = true;
                    var deletedTask = await _repository.DeleteTask(task);
                    if (deletedTask != null)
                    {
                        _logger.LogInformation("Task deleted successfully");

                        return new DTO.Task()
                        {
                            Name = deletedTask.Name,
                            EndDate = deletedTask.EndDate,
                            Description = deletedTask.Description,
                            Importance = deletedTask.Importance,
                            ProjectId = deletedTask.ProjectId,
                        };

                    }
                }
                else
                {
                    _logger.LogError ("Task could not be deleted");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Task();
        }

        public async Task<DTO.Task> GetTaskById(int id)
        {
            try
            {
                var task = await _repository.GetTaskById(id);
                if (task != null)
                {
                    _logger.LogInformation("Task retrieved successfully");

                    return new DTO.Task()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        EndDate = task.EndDate,
                        Description = task.Description,
                        Importance = task.Importance,
                        ProjectId = task.ProjectId,
                    };
                }
                else
                {
                    _logger.LogError("Task could not be added");

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
                var dtoTasks = new List<DTO.Task>();
                if (tasks != null)
                {
                    _logger.LogInformation("Tasks retrieved successfully");

                    tasks.ForEach(x => dtoTasks.Add(new DTO.Task
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EndDate = x.EndDate,
                        Description = x.Description,
                        Importance = x.Importance,
                        ProjectId = x.ProjectId,
                    }));
                    return dtoTasks;
                }
                else
                {
                    _logger.LogError("Task could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<DTO.Task>();
        }

        public async Task<DTO.Task> UpdateTask(DTO.Task model)
        {
            try
            {
                var task = await _repository.GetTaskById(model.Id);
                if (task != null)
                {
                    task.Name = model.Name;
                    task.EndDate = model.EndDate;
                    task.Description = model.Description;
                    task.Importance = model.Importance;
                    task.ProjectId = model.ProjectId;
                    var updated = await _repository.UpdateTask(task);
                    if (updated != null)
                    {
                        _logger.LogInformation("Task updated successfully");

                        return new DTO.Task()
                        {
                            Name = task.Name,
                            EndDate = task.EndDate,
                            Description = task.Description,
                            Importance = task.Importance,
                            ProjectId = task.ProjectId,
                        };
                    }
                    else
                    {
                        _logger.LogError("Task could not be updated");

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
