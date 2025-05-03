namespace IdealSolutionsAssessment.Data;

using Microsoft.EntityFrameworkCore;
using IdealSolutionsAssessment.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.AssignedUser)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.AssignedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
