using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace Releaseasy.Model
{
    public class Role : IdentityRole
    {
        public virtual ICollection<User> Users { get; set; }
    }
}
