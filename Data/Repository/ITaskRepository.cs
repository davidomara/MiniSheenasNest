using MiniSheenasNest.Models;

namespace MiniSheenasNest.Data.Repository
{
    public interface ITaskRepository
    {
        Task<TaskItem> GetTaskByIdAsync(int taskItemId);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task AddTaskAsync(TaskItem task);
        Task UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(int taskItemId);
        Task SaveChangesAsync();
    }
}
