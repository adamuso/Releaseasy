using Microsoft.EntityFrameworkCore;
using Releaseasy.Model;

namespace Releaseasy
{
    public class ReleaseasyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

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
        }
    }
}
