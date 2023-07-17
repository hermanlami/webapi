namespace TaskManagementSystem.DAL.Interfaces
{
    public interface ITasksRepository
    {
        Task<Entities.Task> AddTask(Entities.Task entity);
        Task<List<Entities.Task>> GetTasks();
        Task<Entities.Task> GetTaskById(int id);
        Task<List<Entities.Task>> GetTasksByDeveloperId(int developerId);
        Task<List<Entities.Task>> GetCompletedTasks();
        Task<List<Entities.Task>> GetTasksByProjectId(int id); 
        Task<Entities.Task> UpdateTask(Entities.Task entity);
        Task<Entities.Task> DeleteTask(Entities.Task entity);
        Task<List<Entities.Task>> GetTasksCloseToDeadline();

    }
} 
 
