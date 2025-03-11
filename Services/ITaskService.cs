using MiniSheenasNest.Models;

namespace MiniSheenasNest.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int taskItemId);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(int taskItemId);

        // New method for updating task status via AJAX.
        Task UpdateTaskStatusAsync(int taskId, string newStatus);
    }
}
