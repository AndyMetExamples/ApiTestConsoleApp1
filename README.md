# ApiTestConsoleApp1
Console app to display the average number of words in an artists lyrics. Uses a couple of APIs to get the data...

- Uses https://musicbrainz.org/ to get the songs by a given artist (well, some of them, i didn't find this the most intuitive, so there is room for improvemnt)
- Uses https://lyricsovh.docs.apiary.io/#reference/0/lyrics-of-a-song/search?console=1 to count the words of each song (if it can find them)

As mentioned above, at first glance I didn't find the musicbrainz.org documentation very clear so I thought using one of the musicbrainz NuGet packages might be easier/quicker but the one I picked was not that intuitive (to me) either, So I had to experiment within Postman to figure out a query to get the data required.

To run the app, clone it (if using Visual Studio set the ConsoleApp1 to be the Startup project, other IDE's should be fine too, I created this in jetbrains Rider, which was another learning experience) then run/debug the app.



