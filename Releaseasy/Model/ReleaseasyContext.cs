using Microsoft.EntityFrameworkCore;
using Releaseasy.Model;

namespace Releaseasy
{
    public class ReleaseasyContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Server=DEKU\SQLEXPRESS;Database=ReleaseasyTestDb;Integrated Security=True");
        }
    }
}
