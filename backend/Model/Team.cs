using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Releaseasy.Model
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public Project Project { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<TaskTeam> Tasks { get; set; }
    }
}
