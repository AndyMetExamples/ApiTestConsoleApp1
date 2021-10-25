using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    
    internal class ArtistSearchResponse
    {
        [JsonPropertyName("count")] 
        public int ResultCount { get; set; }
        
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
        
        
        [JsonPropertyName("artists")]
        public ICollection<Artist> Artists { get; set; }
    }
}