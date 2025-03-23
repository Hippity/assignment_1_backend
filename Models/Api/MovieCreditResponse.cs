using System.Composition;
using System.Text.Json.Serialization;

namespace MovieBackend.Models.Api;

public class MovieCreditResponse {

    [JsonPropertyName("id")]
    public int Id { get; set; } 
    [JsonPropertyName("cast")]
    public List<Cast> Cast { get; set; }  = [];
    [JsonPropertyName("crew")]
    public List<Crew> Crew { get; set; }  = [];

}