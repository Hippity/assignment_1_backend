using System.Text.Json.Serialization;

namespace MovieBackend.Models
{
    public class Cast
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; } = true;
        
        [JsonPropertyName("gender")]
        public int Gender { get; set; } = 0;
        
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;
        
        [JsonPropertyName("known_for_department")]
        public string KnownForDepartment { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("original_name")]
        public string OriginalName { get; set; } = string.Empty;
        
        [JsonPropertyName("popularity")]
        public double Popularity { get; set; } = 0;
        
        [JsonPropertyName("profile_path")]
        public string ProfilePath { get; set; } = string.Empty;
        
        [JsonPropertyName("cast_id")]
        public int CastId { get; set; } = 0;
        
        [JsonPropertyName("character")]
        public string Character { get; set; } = string.Empty;
        
        [JsonPropertyName("credit_id")]
        public string CreditId { get; set; } = string.Empty;
        
        [JsonPropertyName("order")]
        public int Order { get; set; } = 0;
    }
}
