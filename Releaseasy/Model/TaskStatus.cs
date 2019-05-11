using System.ComponentModel.DataAnnotations;

namespace Releaseasy.Model
{
    public class TaskStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public Project Project { get; set; }
    }
}