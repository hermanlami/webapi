namespace TaskManagementSystem.DAL.Interfaces
{
    public interface ITasksRepository
    {
        Task<Entities.Task> AddTask(Entities.Task entity);
        Task<List<Entities.Task>> GetTasks(int id=0);
        Task<Entities.Task> GetTaskById(int id, int userId=0);
        Task<Entities.Task> GetTaskByName(string name, int id=0, int userId = 0, string userRole="");

        Task<List<Entities.Task>> GetTasksByDeveloperId(int developerId);
        Task<List<Entities.Task>> GetCompletedTasks();
        Task<List<Entities.Task>> GetTasksByProjectId(int id, int developerId = 0); 
        Task<Entities.Task> UpdateTask(Entities.Task entity);
        Task<Entities.Task> DeleteTask(Entities.Task entity);
        Task<List<Entities.Task>> GetTasksCloseToDeadline();

    }
} 
 
