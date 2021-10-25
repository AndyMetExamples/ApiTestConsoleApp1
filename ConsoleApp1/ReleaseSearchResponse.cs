using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class ReleaseSearchResponse
    {
        [JsonPropertyName("count")] 
        public int ResultCount { get; set; }

        [JsonPropertyName("offset")] 
        public int Offset { get; set; }

        [JsonPropertyName("releases")]
        public ICollection<Release> Releases { get; set; }
    }
}