using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
            ImplicitGrantAuth auth = new ImplicitGrantAuth(_clientId, _redirectUri, serverUri, Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                                                                                             Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.UserFollowRead |
                                                                                             Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                                                                                             Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);
            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop(); // 'sender' is also the auth instance
                SpotifyWebAPI spotifyWebAPI = new SpotifyWebAPI(){ TokenType = payload.TokenType, AccessToken = payload.AccessToken };

                // Do requests with API client
                
                //Get Users Top Artists
                List<FullArtist> topArtists = spotifyWebAPI.GetUsersTopArtists(TimeRangeType.ShortTerm,50,0).Items;

                Console.WriteLine (topArtists.First().Images.First().Url);
            };

            auth.Start(); // Start an internal HTTP Server
            auth.OpenBrowser();
            Console.ReadLine();
        }
    }
}
