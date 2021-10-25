using System;
using System.Linq;
using RestSharp;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Header("Hello, press Ctrl+c to exit...");
            while (true)
            {
                Header("Please enter the name of a band or artist...");
                var artistName = Console.ReadLine();
                var artistSearchResponse = ArtistSearchResponse(artistName);
                DisplaySearchSummary(artistSearchResponse);

                if (artistSearchResponse.Artists.Any(a => a.SearchScore == 100))
                {
                    var releaseSearchResponse = ReleaseSearchResponse(artistName);
                    var theSongs = releaseSearchResponse.Releases
                        .Where(r => r.Score == 100 && r.Country == "GB")
                        .Select(r => r.Title)
                        .Distinct().ToArray();

                    Console.WriteLine($"Found {theSongs.Length} songs.");
                    var songTotal = 0;
                    var wordTotal = 0;

                    foreach (var song in theSongs)
                    {
                        Console.Write($"    {song} : ");
                        var lyricSearchResponse = LyricSearchResponse(artistName, song);
                        CountLyrics(lyricSearchResponse, ref songTotal, ref wordTotal);
                    }

                    DisplayAverageLyricCount(songTotal, wordTotal);
                }
            }
        }

        private static void CountLyrics(LyricSearchResponse lyricSearchResponse, ref int songTotal, ref int wordTotal)
        {
            if (lyricSearchResponse is null)
            {
                Console.WriteLine($"no lyrics found");
            }
            else
            {
                if (lyricSearchResponse.WordCount > 0)
                {
                    Console.WriteLine($"has {lyricSearchResponse.WordCount} words");
                    songTotal += 1;
                    wordTotal += lyricSearchResponse.WordCount;
                }
                else
                {
                    Console.WriteLine($"no lyrics found");
                }
            }
        }

        private static void DisplayAverageLyricCount(int songTotal, int wordTotal)
        {
            if (songTotal == 0)
            {
                Console.WriteLine("No lyrics found");
            }
            else
            {
                Console.WriteLine($"Average words per song is {wordTotal / songTotal}");
            }
        }

        private static LyricSearchResponse LyricSearchResponse(string artistName, string song)
        {
            var lyricUri = $"https://api.lyrics.ovh/v1/{artistName}/{song}";
            var lyricClient = new RestClient(lyricUri) {Timeout = -1};
            var lyricRequest = new RestRequest(Method.GET);
            lyricRequest.AddHeader("Accept", "application/json");
            IRestResponse lyricResponse = lyricClient.Execute(lyricRequest);
            //Console.WriteLine(lyricResponse.Content);

            return JsonSerializer.Deserialize<LyricSearchResponse>(lyricResponse.Content);
        }

        private static ReleaseSearchResponse ReleaseSearchResponse(string artistName)
        {
            var songSearchUri =
                $"http://musicbrainz.org/ws/2/release/?query=artist:{artistName}&group=single&country=GB&status=official";
            var songClient = new RestClient(songSearchUri) {Timeout = -1};
            var songRequest = new RestRequest(Method.GET);
            songRequest.AddHeader("Accept", "application/json");
            IRestResponse songResponse = songClient.Execute(songRequest);
            //Console.WriteLine(songResponse.Content as string);

            return JsonSerializer.Deserialize<ReleaseSearchResponse>(songResponse.Content);
        }

        private static void DisplaySearchSummary(ArtistSearchResponse artistSearchResponse)
        {
            if (artistSearchResponse is null || artistSearchResponse.ResultCount == 0)
            {
                Console.WriteLine("Sorry,no artists found with that name");
            }
            else if (artistSearchResponse.Artists.Any(a => a.SearchScore == 100))
            {
                Console.WriteLine($"Found an exact match, now searching for song titles...");
            }
            else
            {
                Console.WriteLine(
                    $"Did not find an exact match but found {artistSearchResponse.ResultCount} similar names");
            }
        }

        private static ArtistSearchResponse ArtistSearchResponse(string artistName)
        {
            var artistSearchUri = $"http://musicbrainz.org/ws/2/artist/?query=artist:{artistName}";
            var client = new RestClient(artistSearchUri) {Timeout = -1};
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content as string);

            return JsonSerializer.Deserialize<ArtistSearchResponse>(response.Content as string);
        }
        
        private static void Header(string title)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine();

            Console.ForegroundColor = color;
        }

        // private static async Task SearchArtists(HttpClient client, HttpRequestMessage request)
        // {
        //     using (var response = await client.SendAsync(request))
        //     {
        //         response.EnsureSuccessStatusCode();
        //         var body = await response.Content.ReadAsStringAsync();
        //         Console.WriteLine(body);
        //     }
        // }
        //
        // private static async Task RunExamples()
        // {
        //     // Get path for local file cache.
        //     //var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //
        //     var client = new MusicBrainzClient();
        //     //{
        //     //    Cache = new FileRequestCache(Path.Combine(location, "cache"))
        //     //};
        //
        //     Header("Hello, please enter the name of a band or artist...");
        //
        //     var artistName = Console.ReadLine();
        //
        //     // Get the names of the songs from the nuget package
        //     // I thought this was OK for a quick example, why reinvent the wheel?
        //     // but as it turned out it wasn't as userfriendly  to use as i'd hoped!
        //     var songs = await MusicBrainzNugetAdapation.GetSongNames(client, artistName);
        //
        //     using (var lyricClient = new HttpClient())
        //     {
        //         var songTotal = 0;
        //         var wordTotal = 0;
        //
        //         Console.WriteLine($"Found {songs.Length} songs.");
        //
        //         foreach (var song in songs)
        //         {
        //             var requestUri = $"https://api.lyrics.ovh/v1/{artistName}/{song}";
        //             var lyricResponse = await GetLyricResponse(lyricClient, requestUri);
        //
        //             if (lyricResponse is null)
        //             {
        //                 Console.WriteLine($"    {song} : no lyrics found");
        //             }
        //             else
        //             {
        //                 //Console.WriteLine("");
        //                 Console.WriteLine($"    {song} : has {lyricResponse.WordCount} words");
        //                 //Console.WriteLine(lyricResponse.Lyrics);
        //                 songTotal++;
        //                 wordTotal += lyricResponse.WordCount;
        //             }
        //         }
        //
        //         Console.WriteLine($"Average words per song is {wordTotal / songTotal}");
        //     }
        // }
        //
        // private static async Task<LyricSearchResponse> GetLyricResponse(HttpClient client, string uri)
        // {
        //     try
        //     {
        //         return await client.GetFromJsonAsync<LyricSearchResponse>(uri);
        //     }
        //     catch (HttpRequestException ex1)
        //     {
        //         // lets hide the errors as the spoil the appearance of the output
        //         Console.WriteLine("An error occurred");
        //     }
        //     catch (NotSupportedException ex2)
        //     {
        //         // lets hide the errors as the spoil the appearance of the output
        //         Console.WriteLine("Content type is not supported.");
        //     }
        //     catch (JsonException ex3)
        //     {
        //         // lets hide the errors as the spoil the appearance of the output
        //         Console.WriteLine("Invalid JSON");
        //     }
        //
        //     return null;
        // }
        //
        // private static async Task<ArtistSearchResponse> GetArtistSearchResponse(HttpClient client, string uri)
        // {
        //     try
        //     {
        //         return await client.GetFromJsonAsync<ArtistSearchResponse>(uri);
        //     }
        //     catch (HttpRequestException ex1)
        //     {
        //         // lets hide the errors as the spoil the appearance of the output
        //         Console.WriteLine("An error occurred");
        //     }
        //     catch (NotSupportedException ex2)
        //     {
        //         // lets hide the errors as the spoil the appearance of the output
        //         Console.WriteLine("Content type is not supported.");
        //     }
        //     catch (JsonException ex3)
        //     {
        //         // lets hide the errors as the spoil the appearance of the output
        //         Console.WriteLine("Invalid JSON");
        //     }
        //
        //     return null;
        // }
    }
}