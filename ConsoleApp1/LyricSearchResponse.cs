using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    public class LyricSearchResponse
    {
        [JsonPropertyName("lyrics")] 
        public string Lyrics { get; set; }

        [JsonPropertyName("error")] 
        public string Error { get; set; }

        private int? _countedWords { get; set; }

        /// <summary>
        ///     This isn't quite right as there appears to be an initial/header line before the actual lyrics,
        ///     we should really look for that and remove it from the count.
        /// </summary>
        public int WordCount
        {
            get
            {
                // Have we already counted the lyrics?
                if (_countedWords.HasValue)
                    return _countedWords.Value;

                // if not lets, count them now.
                if (string.IsNullOrWhiteSpace(Lyrics))
                {
                    _countedWords = 0;
                }
                else
                {
                    _countedWords = Lyrics.Split(" ").Length;
                }

                return _countedWords.Value;
            }
        }
    }
}