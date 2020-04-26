using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Releaseasy.Model
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }
    }
}
