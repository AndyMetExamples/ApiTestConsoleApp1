using System;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class Release
    {
        [JsonPropertyName("id")] 
        public Guid Id { get; set; }

        [JsonPropertyName("title")] 
        public string Title { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
        
        [JsonPropertyName("count")] 
        public int ReleaseCount { get; set; }
        
        [JsonPropertyName("score")]
        public int Score { get; set; }
    }
}