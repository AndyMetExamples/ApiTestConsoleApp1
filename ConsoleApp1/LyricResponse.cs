using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class LyricResponse
    {
        [JsonPropertyName("lyrics")] public string Lyrics { get; set; }

        [JsonPropertyName("error")] public string Error { get; set; }

        private int _wordCount { get; set; }

        /// <summary>
        ///     This isn't quite right as there appears to be an initial/header line before the actual lyrics,
        ///     we should really look for that and remove it from the count.
        /// </summary>
        public int WordCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Lyrics))
                    return 0;
                _wordCount = Lyrics.Split(" ").Length;

                return _wordCount;
            }
        }
    }
}