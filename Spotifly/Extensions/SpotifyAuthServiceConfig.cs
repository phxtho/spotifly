using Hanssens.Net;
using Microsoft.Extensions.DependencyInjection;
using Spotifly.Models;
using System;
using System.Threading.Tasks;

namespace Spotifly.Extensions
{
    public static class SpotifyAuthServiceConfig
    {
        public static void AddSpotifyAuthConfig(this IServiceCollection services)
        {
            //Setup spotify authentication
            services.AddAuthentication()
           .AddSpotify(options =>
           {
               options.ClientId = "8fed9a69a36f416e9d1069c3a74f23f3";
               options.ClientSecret = "10d488eca54e4992b2eeeb9f6403b090";
               options.SaveTokens = true;
               options.CallbackPath = "/callback";
               options.Events.OnRemoteFailure = (context) =>
               {
                   // context.Response.Redirect(context.Properties.GetString("returnUrl"));
                   // context.HandleResponse();
                   return Task.CompletedTask;
               };
               options.Scope.Add("user-read-recently-played");
               options.Scope.Add("user-read-email");
               options.Scope.Add("user-library-read");

               options.Events.OnCreatingTicket = (ctx) =>
               {
                   using (var storage = new LocalStorage())
                   {
                       // persist tokens used for api requests
                       // Todo : Is this save? any other methods? 
                       // Todo: Handle token expiration
                       var token = new ExternalToken
                       {
                           AccessToken = ctx.AccessToken,
                           RefreshToken = ctx.RefreshToken,
                           TokenType = ctx.TokenType
                       };

                       storage.Store("token", token);
                       storage.Persist();

                   }
                   return Task.CompletedTask;
               };



           });
        }
    }
}
