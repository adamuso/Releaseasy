using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Releaseasy.Model
{
    public class TaskTeam
    {
        public int TaskId { get; set; }
        public int TeamId { get; set; }

        public Task Task { get; set; }
        public Team Team { get; set; }
    }
}
