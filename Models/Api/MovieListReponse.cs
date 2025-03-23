using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieBackend.Models.Api;

    public class MovieListResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("results")]
        public List<MoveListDetails> Results { get; set; } = [];
        
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        
        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
