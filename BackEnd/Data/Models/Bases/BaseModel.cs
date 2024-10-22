using Data.Interface;
using System.Text.Json.Serialization;

namespace Data.Models.Bases
{
    public abstract class BaseModel : IDelete
    {
        [JsonIgnore]
        public int Version { get; internal set; }
        [JsonIgnore]
        public string? CreatedById { get; internal set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; internal set; }
        [JsonIgnore]
        public string? LastModifiedById { get; internal set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; internal set; }
        [JsonIgnore]
        public DateTime? Deleted { get; set; }
    }
}
