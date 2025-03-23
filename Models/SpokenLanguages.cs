


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieBackend.Models;

public class SpokenLanguage {
    [JsonPropertyName("iso_639_1")]
    public string Iso_639_1 { get; set; } = string.Empty;
    [JsonPropertyName("english_name")]
    public string EnglishName { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}