using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Releaseasy.Model
{
    public class TaskGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public Project Project { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}