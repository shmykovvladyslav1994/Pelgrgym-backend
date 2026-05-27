using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }

        // Таблица Tasks в базе данных
        public DbSet<TaskItem> Tasks { get; set; }
        // users
        public DbSet<User> Users { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingSet> TrainingSets { get; set; }
    }
}
