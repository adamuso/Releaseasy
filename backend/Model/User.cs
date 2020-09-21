using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace Releaseasy.Model
{
    public class User : IdentityUser
    {
        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<ProjectUser> Projects { get; set; }
        public Role Role { get; set; }
    }
}