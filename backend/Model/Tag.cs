using System.Collections.Generic;

namespace Releaseasy.Model
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TaskTag> Tasks { get; set; }
    }
}