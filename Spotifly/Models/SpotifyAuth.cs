using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotifly.Models
{
    public class SpotifyAuth
    {
        //App details specified on my spotify developer dashboard
        private static string _clientId = "82bf63bc11194431afc9b8ad2f245bd3";
        private static string _redirectUri = "http://localhost:4002";
        private static string serverUri = "http://localhost:4002";

        private ISpotifyWebAPI _spotifyWebAPI;
        public SpotifyAuth(ISpotifyWebAPI spotifyWebAPI)
        {
            _spotifyWebAPI = spotifyWebAPI;

            ImplicitGrantAuth auth = new ImplicitGrantAuth(_clientId, _redirectUri, serverUri, Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                                                                                             Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.UserFollowRead |
                                                                                             Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                                                                                             Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);
            auth.AuthReceived += AuthOnAuthReceived;
            auth.Start();
            auth.OpenBrowser();
        }

        private void AuthOnAuthReceived(object sender, Token payload)
        {
            ImplicitGrantAuth auth = (ImplicitGrantAuth)sender;

            _spotifyWebAPI.TokenType = payload.TokenType;
            _spotifyWebAPI.AccessToken = payload.AccessToken;
        }
    }
}
