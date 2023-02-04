using Microsoft.EntityFrameworkCore;
using TaskRegister.API.Entities.Projects;
using TaskRegister.API.Entities.TaskRegister;

namespace TaskRegister.API.DbContexts
{
    public class TaskRegisterContext : DbContext
    {
        public TaskRegisterContext(DbContextOptions<TaskRegisterContext> options) 
            : base(options)
        {
            
        }

        public DbSet<BaseProjects> Projects { get; set; }
        public DbSet<ProductionTimeSlip> ProductionTimeSlips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseProjects>()
                .HasIndex(i => i.Project)
                .IsUnique();

            modelBuilder.Entity<ProductionTimeSlip>()
                .HasIndex(i => i.Subject)
                .IsUnique();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
