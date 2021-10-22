using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Hqub.MusicBrainz.API;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Make sure that TLS 1.2 is available.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            try
            {
                var task = RunExamples();

                task.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        private static async Task RunExamples()
        {
            // Get path for local file cache.
            //var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var client = new MusicBrainzClient();
            //{
            //    Cache = new FileRequestCache(Path.Combine(location, "cache"))
            //};

            Header("Hello, please enter the name of a band or artist...");

            var artistName = Console.ReadLine();

            // Get the names of the songs from the nuget package
            // I thought this was OK for a quick example, why reinvent the wheel?
            // but as it turned out it wasn't as userfriendly  to use as i'd hoped!
            var songs = await MusicBrainzNugetAdapation.GetSongNames(client, artistName);

            using (var lyricClient = new HttpClient())
            {
                var songTotal = 0;
                var wordTotal = 0;

                Console.WriteLine($"Found {songs.Length} songs.");

                foreach (var song in songs)
                {
                    var requestUri = $"https://api.lyrics.ovh/v1/{artistName}/{song}";
                    var lyricResponse = await GetLyricResponse(lyricClient, requestUri);

                    if (lyricResponse is null)
                    {
                        Console.WriteLine($"    {song} : no lyrics found");
                    }
                    else
                    {
                        //Console.WriteLine("");
                        Console.WriteLine($"    {song} : has {lyricResponse.WordCount} words");
                        //Console.WriteLine(lyricResponse.Lyrics);
                        songTotal++;
                        wordTotal += lyricResponse.WordCount;
                    }
                }

                Console.WriteLine($"Average words per song is {wordTotal / songTotal}");
            }
        }

        private static async Task<LyricResponse> GetLyricResponse(HttpClient client, string uri)
        {
            try
            {
                return await client.GetFromJsonAsync<LyricResponse>(uri);
            }
            catch (HttpRequestException ex1)
            {
                // lets hide the errors as the spoil the appearance of the output
                //Console.WriteLine("An error occurred");
            }
            catch (NotSupportedException ex2)
            {
                // lets hide the errors as the spoil the appearance of the output
                // Console.WriteLine("Content type is not supported."); 
            }
            catch (JsonException ex3)
            {
                // lets hide the errors as the spoil the appearance of the output
                // Console.WriteLine("Invalid JSON"); 
            }

            return null;
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
    }
}