using Microsoft.EntityFrameworkCore;
using MiniSheenasNest.Models;
using System;

namespace MiniSheenasNest.Data.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTaskAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
        }

        public async Task DeleteTaskAsync(int taskItemId)
        {
            var task = await _context.TaskItems.FindAsync(taskItemId);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int taskItemId)
        {
            return await _context.TaskItems.FindAsync(taskItemId);
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
