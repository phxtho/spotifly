using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using System;
using System.Collections.Generic;

namespace TestAPI
{ 

    class Program
    {
        private static string _clientId = "82bf63bc11194431afc9b8ad2f245bd3";
        private static string _redirectUri = "http://localhost:4002";
        private static string serverUri = "http://localhost:4002";

        static void Main(string[] args)
        {
            // Authentication first
            ImplicitGrantAuth auth = new ImplicitGrantAuth(_clientId, _redirectUri, serverUri, Scope.UserTopRead);
            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop(); // 'sender' is also the auth instance
                SpotifyWebAPI api = new SpotifyWebAPI(){ TokenType = payload.TokenType, AccessToken = payload.AccessToken };

                // Do requests with API client

                TuneableTrack tar = new TuneableTrack();
                tar.Popularity = 100;
                Recommendations rec = api.GetRecommendations(genreSeed: new List<string> { "rock", "rap" }, target: tar, market: "US");
                rec.Tracks.ForEach(track => {
                    Console.Write(track.Name + " : ");
                    track.Artists.ForEach(artist => Console.Write(artist.Name + "\n"));
                });
            };

            auth.Start(); // Start an internal HTTP Server
            auth.OpenBrowser();
            Console.ReadLine();
        }
    }
}
