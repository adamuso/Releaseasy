using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace Releaseasy.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<ProjectUser> Projects { get; set; }
    }
}