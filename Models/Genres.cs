


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieBackend.Models;

public class Genre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

}