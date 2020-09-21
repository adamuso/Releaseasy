using Microsoft.EntityFrameworkCore;
using Releaseasy.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Releaseasy
{
    public class ReleaseasyContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ReleaseasyContext(DbContextOptions options) : base(options)
        {
        }

        public ReleaseasyContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Project

            modelBuilder.Entity<Project>()
                .HasOne(project => project.Creator)
                .WithMany(user => user.CreatedProjects);

            modelBuilder.Entity<ProjectUser>()
                .HasKey("ProjectId", "UserId");

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(project => project.Users);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(user => user.Projects);

            #endregion

            #region Task
            //TASK-TAG
            modelBuilder.Entity<TaskTag>()
                .HasKey("TaskId", "TagId");

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany(task => task.Tags);

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(tag => tag.Tasks);
            #endregion

            #region Tag
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name).IsUnique();
            #endregion
        }
    }
}
