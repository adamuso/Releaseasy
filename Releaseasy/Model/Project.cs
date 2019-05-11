using System;
using System.Collections.Generic;

namespace Releaseasy.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Creator { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<Team> Teams { get; set; }
        //public virtual ICollection<TaskStatus> TaskStatuses { get; set; }
        //public virtual ICollection<TaskGroup> TaskGroups { get; set; }
    }
}
