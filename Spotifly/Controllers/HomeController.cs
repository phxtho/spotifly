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

//ChartJs
using ChartJSCore.Models;

namespace Spotifly.Controllers
{
    public class HomeController : Controller
    {
        private ISpotifyWebAPI _spotifyWebAPI;

        public HomeController(ISpotifyWebAPI spotifyWebAPI)
        {
            _spotifyWebAPI = spotifyWebAPI;
        }

        public IActionResult Index()
        {
            // Authentication first
           /* ImplicitGrantAuth auth = new ImplicitGrantAuth(_clientId, _redirectUri, serverUri, Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | 
                                                                                             Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.UserFollowRead | 
                                                                                             Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                                                                                             Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);
            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop(); // 'sender' is also the auth instance
               // _spotifyWebAPI. { TokenType = payload.TokenType, AccessToken = payload.AccessToken };
            };

            auth.Start(); // Start an internal HTTP Server
            auth.OpenBrowser();*/


            auth.OpenBrowser();
            ViewData["Users"] = Models.User.SelectAll();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            Chart pieChart = GeneratePieChart();

            ViewData["PieChart"] = pieChart;

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

        public static Chart GeneratePieChart()
        {
            Chart chart = new Chart();
            chart.Type = "pie";

            Data data = new Data();
            data.Labels = new List<string>() { "Red", "Blue", "Yellow" };

            PieDataset dataset = new PieDataset()
            {
                Label = "My dataset",
                BackgroundColor = new List<string>() { "#FF6384", "#36A2EB", "#FFCE56" },
                HoverBackgroundColor = new List<string>() { "#FF6384", "#36A2EB", "#FFCE56" },
                Data = new List<double>() { 300, 50, 100 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }
    }
}
