


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieBackend.Models;

public class ProductionCompany {

    [JsonPropertyName("id")]
    public int Id {get; set;}
    [JsonPropertyName("logo_path")]
    public string LogoPath {get; set;}  = string.Empty;
    [JsonPropertyName("name")]
    public string Name {get; set;}  = string.Empty;
    [JsonPropertyName("origin_country")]
    public string OriginCountry {get; set;}  = string.Empty;
    
}