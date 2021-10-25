using System;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class Artist
    {
        [JsonPropertyName("id")] 
        public Guid Id { get; set; }

        [JsonPropertyName("name")] 
        public string ArtistName { get; set; }
        
        [JsonPropertyName("type")]
        public string ArtistType { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
        
        [JsonPropertyName("score")]
        public int SearchScore { get; set; }
        
        public string[] Songs { get; set; }
    }
}