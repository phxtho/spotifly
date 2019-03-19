using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//SpotifyAPI-NET
using Spotifly.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;

namespace Spotifly.Controllers
{
    public class HomeController : Controller
    {
        //App details specified on my spotify developer dashboard
        private static string _clientId = "82bf63bc11194431afc9b8ad2f245bd3";
        private static string _redirectUri = "http://localhost:4002";
        private static string serverUri = "http://localhost:4002";

        public IActionResult Index()
        {
            // Authentication first
            ImplictGrantAuth auth = new ImplictGrantAuth(_clientId, _redirectUri, serverUri, Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | 
                                                                                             Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.UserFollowRead | 
                                                                                             Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                                                                                             Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

            auth.Start(); // Start an internal HTTP Server
            auth.OpenBrowser();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Statistics()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
