using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSheenasNest.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    [Table("TaskItems")]
    public class TaskItem
    {
        public int TaskItemId { get; set; }         // Primary key

        public string Title { get; set; }           // Task title

        public string Description { get; set; }     // Task details

        public TaskStatus Status { get; set; }      // E.g., ToDo, InProgress, Done

        public TaskPriority Priority { get; set; }  // E.g., Low, Medium, High

        public DateTime DueDate { get; set; }       // When the task is due

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Optionally: assign to a user (for ASP.NET Identity, you might use string)
        public string AssignedUserId { get; set; }
    }
}
