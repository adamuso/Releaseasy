using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Releaseasy.Model
{
    public class Role : IdentityRole
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
