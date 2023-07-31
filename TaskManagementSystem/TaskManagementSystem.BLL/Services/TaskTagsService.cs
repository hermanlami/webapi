using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common.CustomExceptions;
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
        /// <summary>
        /// Shton nje lidhje te re mes nje task-u dhe nje tag-u.
        /// </summary>
        /// <param name="model">Modeli ne baze te te cilit behet shtimi.</param>
        /// <returns>Task-tagun e ri.</returns>
        public async Task<TaskTag> AddTaskTag(TaskTag model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var addedTaskTag = await _repository.AddTaskTag(_mapper.Map<DAL.Entities.TaskTag>(model));
                if (addedTaskTag.Id > 0)
                {
                    Log.Information($"Task tag added successfully");
                    return _mapper.Map<TaskTag>(addedTaskTag);
                }
                Log.Information("Task tag could not be added");
                return new TaskTag(); 
            });
        }

        /// <summary>
        /// Merr nje task-tag ne baze te Id se tag.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar tag-un.</param>
        /// <returns>Task-tagun perkates.</returns>
        public async Task<List<TaskTag>> GetTaskTagByTagId(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var task = await _repository.GetTaskTagByTagId(id);
                if (task != null)
                {
                    Log.Information("Task tag retrieved successfully");
                    return _mapper.Map<List<DTO.TaskTag>>(task);
                }

                Log.Information("Task tag could not be retrieved");
                return new List<TaskTag>();
                

            });
        }

        /// <summary>
        /// Fshin te gjithe tag-et e nje task-u.
        /// </summary>
        /// <param name="id">Id qe sherben per identifikimin e task-ut.</param>
        /// <returns>Task-un e fshire.</returns>
        public async Task<bool> DeleteTaskTags(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var deletedTask = await _repository.DeleteTaskTag(id);
                if (deletedTask)
                {
                    Log.Information($"Tags deleted successfully");
                    return true;
                }
                Log.Error($"Tags could not be deleted");
                throw new CustomException("Tags could not be deleted");

            });
        }
    }
}
