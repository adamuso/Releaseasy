﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Releaseasy.Model
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public TaskGroup Group { get; set; }
        public TaskStatus Status { get; set; }
        public virtual ICollection<User> AssignedUsers { get; set; }
        public virtual ICollection<Team> AssignedTeams { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}