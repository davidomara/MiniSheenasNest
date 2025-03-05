using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSheenasNest.Models
{
    [Table("Messages")]
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public string? SenderName { get; set; } // e.g. "Sheena" or "Nord"

        [Required]
        public string? ReceiverName { get; set; } // e.g. "Sheena" or "Nord"

        [Required]
        public string? Content { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.Today;

        public DateTime? ReadAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        // Potential Enhancement: Handle file attachments
        //public string? AttachmentUrl { get; set; }
    }
}
