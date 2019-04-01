using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using Newtonsoft.Json;

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

        public static bool RegisterUser(string email, string password1, string password2, DateTime dateCreated, ISession sess)
        {
            User user = null;
            try
            {
                user = User.SelectByEmailForAuth(email);
            }
            #pragma warning disable 0168
            catch (Exception e) { }
            #pragma warning restore 0168
            if (user != null)
                throw new Exception("User already exists");
            if (password1 == password2)
            {
                Token token = GenerateUserToken();
                if (token != null)
                {
                    ISpotifyWebAPI api = EndpointFromToken(token);
                    string name = api.GetPrivateProfile().DisplayName;
                    string hashed = HashPassword(password1, dateCreated);
                    user = User.InsertUser(name, email, hashed, dateCreated);
                    UserToken.InsertToken(user.Id, token.AccessToken, token.RefreshToken, token.CreateDate);
                    sess.SetString("userId", user.Id.ToString());
                    sess.SetString("userName", user.Name);
                    sess.SetString("token", JsonifyToken(token));
                    return true;
                }
                else
                    throw new Exception("Failed to perform token exchange");
            }
            return false;
        }

        public static bool LogInUser(string email, string password, ISession sess)
        {
            User user = User.SelectByEmailForAuth(email);
            string hashed = HashPassword(password, user.DateCreated);
            UserToken userToken = null;
            Token token = null;

            if (hashed == user.Password)
            {
                sess.SetString("userId", user.Id.ToString());
                sess.SetString("userName", user.Name);
                Console.WriteLine(user.Id.ToString());
                userToken = UserToken.SelectToken(user.Id);
                Console.WriteLine(userToken.DateCreated.ToString());
                Console.WriteLine("LOGIN");
                token = new Token {
                    AccessToken = userToken.AccessToken,
                    RefreshToken = userToken.RefreshToken,
                    ExpiresIn = 3600,
                    CreateDate = userToken.DateCreated
                };
                MaybeRefreshToken(user.Id, ref token);
                sess.SetString("token", JsonifyToken(token));
                return true;
            }
            return false;
        }

        private static string HashPassword(string password, DateTime dateTime)
        {
            string dateTimeStr = dateTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        
        public static Token GenerateUserToken()
        {
            const int MaxTries = 600; // 2 Mins
            int TryCount = 0;
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
            while (tokenTask == null && TryCount < MaxTries)
            {
                Thread.Sleep(200);
                ++TryCount;
            }
            tokenTask.Wait(120000);
            Console.WriteLine($"{DateTime.Now.ToString()} - Done");
            return tokenTask.Result;
        }

        public static ISpotifyWebAPI FetchUserEndpoint(ISession sess)
        {
            Int64 userId = Int64.Parse(sess.GetString("userId"));
            Token token = DesjonifyToken(sess.GetString("token"));

            if (MaybeRefreshToken(userId, ref token))
            {
                sess.SetString("token", JsonifyToken(token));
            }
            return EndpointFromToken(token);
        }

        private static Token RefreshToken(Int64 userId, string refreshToken)
        {
            Token token = null;
            AuthorizationCodeAuth auth = new AuthorizationCodeAuth(_clientId, _secretId, _redirectUri, serverUri, _scope);
            Task<Token> tokenTask = auth.RefreshToken(refreshToken);
            tokenTask.Wait();
            token = tokenTask.Result;
            UserToken.UpdateToken(userId, token.AccessToken, token.RefreshToken, token.CreateDate);
            return token;
        }

        private static bool MaybeRefreshToken(Int64 userId, ref Token token)
        {
            Console.WriteLine("MaybeRefresh");
            Console.WriteLine($"{token.CreateDate.ToString()}");
            if (token.IsExpired())
            {
                token = RefreshToken(userId, token.RefreshToken);
                return true;
            }
            return false;
        }

        private static ISpotifyWebAPI EndpointFromToken(Token token)
        {
            return new SpotifyWebAPI {
                AccessToken = token.AccessToken,
                TokenType = "Bearer"
            };
        }

        public static string JsonifyToken(Token token)
        {
            return JsonConvert.SerializeObject(token);
        }

        public static Token DesjonifyToken(string token)
        {
            return JsonConvert.DeserializeObject<Token>(token);
        }
    }
}
