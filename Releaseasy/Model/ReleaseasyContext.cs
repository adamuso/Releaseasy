using Microsoft.EntityFrameworkCore;
using Releaseasy.Model;

namespace Releaseasy
{
    public class ReleaseasyContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ReleaseasyContext(DbContextOptions options)
            : base(options)
        {
               
        }
    }
}
