﻿namespace Releaseasy.Model
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
    }
}
