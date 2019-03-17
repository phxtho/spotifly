using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using System;

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
            ImplictGrantAuth auth = new ImplictGrantAuth(_clientId, _redirectUri, serverUri, Scope.UserTopRead);
            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop(); // 'sender' is also the auth instance
                SpotifyWebAPI api = new SpotifyWebAPI(){ TokenType = payload.TokenType, AccessToken = payload.AccessToken };

                // Do requests with API client
                Paging<FullTrack> histories =  api.GetUsersTopTracks();
                histories.Items.ForEach(item => Console.WriteLine(item.Name));
            };

            auth.Start(); // Start an internal HTTP Server
            auth.OpenBrowser();
            Console.ReadLine();
        }
    }
}
