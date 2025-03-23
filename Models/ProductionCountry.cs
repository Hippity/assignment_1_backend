


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieBackend.Models;

public class ProductionCountry
{
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string? Name { get; set; } = string.Empty;
}