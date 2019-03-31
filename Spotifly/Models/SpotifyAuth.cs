using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MySql.Data.MySqlClient;
using Dapper;

namespace Spotifly.Models
{
    public class SpotifyAuth
    {
        //App details specified on my spotify developer dashboard
        private static string _clientId = "45bf0233fca5421aa2e7dacdbb338784";

        private static string _secretId = "6f2b0b24a6fb438aa27859d1713baa2d";
        private static string _redirectUri = "http://localhost:4002";
        private static string serverUri = "http://localhost:4002";

        private static Scope _scope = Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                                            Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.UserFollowRead |
                                            Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                                            Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState;

        private ISpotifyWebAPI _spotifyWebAPI;
        private static void MaybeRefreshToken(Int64 userId, ref Token token)
        {
            if (token.IsExpired())
            {
                AuthorizationCodeAuth auth = new AuthorizationCodeAuth(_clientId, _secretId, _redirectUri, serverUri, _scope);

                Task<Token> tokenTask = auth.RefreshToken(token.RefreshToken);
                tokenTask.Wait();
                token = tokenTask.Result;
                UserToken.UpdateToken(userId, token.AccessToken, token.RefreshToken, token.CreateDate);
            }
        }

        public static Token CreateUserToken(Int64 userId)
        {
            AuthorizationCodeAuth auth = new AuthorizationCodeAuth(_clientId, _secretId, _redirectUri, serverUri, _scope);
            Task<Token> tokenTask = null;
            auth.AuthReceived += (sender, payload) =>
            {
                auth.Stop();
                tokenTask = auth.ExchangeCode(payload.Code);
            };
            auth.Start();
            auth.OpenBrowser();
            Console.WriteLine($"{DateTime.Now.ToString()} - Waiting");
            // Plz don't judge me for this awful hack
            while (tokenTask == null)
            {
                Thread.Sleep(200);
            }
            tokenTask.Wait();
            Console.WriteLine($"{DateTime.Now.ToString()} - Done");
            return tokenTask.Result;
        }

        public static ISpotifyWebAPI EndpointFromToken(Token token)
        {
            return new SpotifyWebAPI {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };
        }
        public SpotifyAuth(ISpotifyWebAPI spotifyWebAPI)
        {
            _spotifyWebAPI = spotifyWebAPI;
          
            AuthorizationCodeAuth auth = new AuthorizationCodeAuth(_clientId, _secretId, _redirectUri, serverUri, _scope);
            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop();
                Token token = await auth.ExchangeCode(payload.Code);
                Token refresh = await auth.RefreshToken(payload.Code);
                SpotifyWebAPI api = new SpotifyWebAPI() {TokenType = token.TokenType, AccessToken = token.AccessToken};
                Console.WriteLine("-- AccessToken --");
                Console.WriteLine(api.AccessToken);
                Console.WriteLine(token.RefreshToken);
                Console.WriteLine("Expires In: " + token.ExpiresIn.ToString());
                Console.WriteLine("-- AccessToken --");
                // Do requests with API client
            };
            auth.Start();
            auth.OpenBrowser();
        }

        private void AuthOnAuthReceived(object sender, AuthorizationCode payload)
        {
            ImplicitGrantAuth auth = (ImplicitGrantAuth)sender;

            _spotifyWebAPI.AccessToken = payload.Code;
            
        }
    }
}
