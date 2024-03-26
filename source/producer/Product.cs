using System.Text.Json.Serialization;

namespace producer;

public class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
