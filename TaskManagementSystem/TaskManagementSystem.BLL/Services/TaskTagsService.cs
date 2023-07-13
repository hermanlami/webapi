﻿using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TaskTagsService : ITaskTagsService
    {
        private readonly ITaskTagsRepository _repository;
        private readonly IMapper _mapper;
        public TaskTagsService(ITaskTagsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TaskTag> AddTaskTag(TaskTag model)
        {
            try
            {
                var addedTaskTag = await _repository.AddTaskTag(_mapper.Map<DAL.Entities.TaskTag>(model));
                if (addedTaskTag.Id > 0)
                {
                    Log.Information($"Task tag added successfully");
                    return _mapper.Map<TaskTag>(addedTaskTag);
                }

                Log.Information("Task tag could not be added");

            }
            catch (Exception ex)
            {

            }
            return new DTO.TaskTag();
        }

        public async Task<TaskTag> DeleteTaskTag(int id)
        {
            try
            {
                var taskTag = await _repository.GetTaskTagById(id);
                if (taskTag == null)
                {
                    Log.Information("Task tag could not be deleted");
                    return new DTO.TaskTag();

                }
                var deletedTaskTag = await _repository.DeleteTaskTag(taskTag);
                if (deletedTaskTag != null)
                {
                    Log.Information("Task tag deleted successfully");
                    return _mapper.Map<TaskTag>(deletedTaskTag);
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
                    Log.Information("Task tag retrieved successfully");
                    return _mapper.Map<TaskTag>(task);
                }

                Log.Information("Task tag could not be retrieved");
            }
            catch (Exception ex)
            {

            }
            return new TaskTag();
        }

        public async Task<List<TaskTag>> GetTaskTagByTagId(int id)
        {
            try
            {
                var task = await _repository.GetTaskTagByTagId(id);
                if (task != null)
                {
                    Log.Information("Task tag retrieved successfully");
                    return _mapper.Map<List<DTO.TaskTag>>(task);
                }

                Log.Information("Task tag could not be retrieved");

            }
            catch (Exception ex)
            {

            }
            return new List<TaskTag>();
        }

        public async Task<List<TaskTag>> GetTaskTags()
        {
            try
            {
                var taskTags = await _repository.GetTaskTags();
                if (taskTags != null)
                {
                    Log.Information("Task tags retrieved successfully");
                    return _mapper.Map<List<TaskTag>>(taskTags);
                }

                Log.Information("Task tags could not be retrieved");

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
                if (taskTag == null)
                {
                    Log.Information("Task tag not found");
                    return new DTO.TaskTag();
                }
                var updated = await _repository.UpdateTaskTag(_mapper.Map<DAL.Entities.TaskTag>(model));
                if (updated != null)
                {
                    Log.Information("Task tag updated successfully");
                    return _mapper.Map<TaskTag>(updated);
                }
                Log.Information("Task tag could not be updated");

            }
            catch (Exception ex)
            {

            }
            return new DTO.TaskTag();
        }
    }
}
