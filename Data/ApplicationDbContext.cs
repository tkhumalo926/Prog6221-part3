using Microsoft.EntityFrameworkCore;
using Prog_6221_Part3_PoE.Models;

namespace Prog_6221_Part3_PoE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<ActivityLog> Logs { get; set; }

        public ApplicationDbContext()
        {
            // This magic line creates the database file automatically if it's missing!
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}
