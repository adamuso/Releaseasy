using System.Text.Json.Serialization;

namespace Releaseasy.Model
{
    public class TaskTag
    {
        public int TaskId { get; set; }
        public int TagId { get; set; }
        [JsonIgnore]
        public Task Task { get; set; }
        [JsonIgnore]
        public Tag Tag { get; set; }
    }
}
