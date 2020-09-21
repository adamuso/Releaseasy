using System;
using System.Collections.Generic;

namespace Releaseasy.Model
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public virtual ICollection<TaskTag> Tags { get; set; }
        public User Creator { get; set; } //ID Usera
    }
}
