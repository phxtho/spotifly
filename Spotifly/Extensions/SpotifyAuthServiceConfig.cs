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
               options.ClientSecret = "3a30b3bcac2c4ba6b42630057189a38a";
               options.SaveTokens = true;
               options.CallbackPath = "/callback";
               options.Events.OnRemoteFailure = (context) =>
               {
                   // context.Response.Redirect(context.Properties.GetString("returnUrl"));
                   // context.HandleResponse();
                   return Task.CompletedTask;
               };

               options.Events.OnCreatingTicket = (ctx) =>
               {

                   Console.WriteLine($"{ctx}");
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
