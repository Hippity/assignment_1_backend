


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieBackend.Models;

public class Collection
{
    [JsonPropertyName("id")]
    public int Id { get; set; } 
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; } = string.Empty;
    [JsonPropertyName("backdrop_path")]
    public string BackdropPath { get; set; } = string.Empty;

    public Collection() {
        this.Id = Id;
        this.Name = Name;
        this.PosterPath = PosterPath;
        this.BackdropPath = BackdropPath;
    }
}