using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniSheenasNest.Models;

namespace MiniSheenasNest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Messages> Messages { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
