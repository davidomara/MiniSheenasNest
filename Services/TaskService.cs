using MiniSheenasNest.Data.Repository;
using MiniSheenasNest.Models;

namespace MiniSheenasNest.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;

        public TaskService(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            // Basic validation: ensure Title is provided, etc.
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty.");

            // Set default status if not provided
            if (!Enum.IsDefined(typeof(Models.TaskStatus), task.Status))
                task.Status = Models.TaskStatus.ToDo;

            await _taskRepo.AddTaskAsync(task);
            await _taskRepo.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTaskAsync(int taskItemId)
        {
            await _taskRepo.DeleteTaskAsync(taskItemId);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _taskRepo.GetAllTasksAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int taskItemId)
        {
            return await _taskRepo.GetTaskByIdAsync(taskItemId);
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepo.UpdateTaskAsync(task);
            await _taskRepo.SaveChangesAsync();
        }

        // New method for updating a task's status.
        public async Task UpdateTaskStatusAsync(int taskId, string newStatus)
        {
            // Retrieve the task by its ID.
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            // Attempt to parse the newStatus string to the TaskStatus enum (case-insensitive).
            if (!Enum.TryParse<Models.TaskStatus>(newStatus, true, out var parsedStatus))
            {
                throw new ArgumentException("Invalid status value provided.");
            }

            // Update the task status and the updated timestamp.
            task.Status = parsedStatus;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepo.UpdateTaskAsync(task);
            await _taskRepo.SaveChangesAsync();
        }
    }
}