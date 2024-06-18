using Newtonsoft.Json;

namespace Application.Core.Entities;

public abstract class BaseEntity
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
