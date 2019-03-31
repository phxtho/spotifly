using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

//SpotifyAPI-NET
using Spotifly.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;

//ChartJs
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;
using Hanssens.Net;

namespace Spotifly.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ISpotifyWebAPI _spotifyWebAPI;


        public HomeController(ISpotifyWebAPI spotifyWebAPI)
        {
            _spotifyWebAPI = spotifyWebAPI;

        }


        public IActionResult Index()
        {
            if (_spotifyWebAPI.AccessToken == null)
            {
                SpotifyAuth authenticate = new SpotifyAuth(_spotifyWebAPI);
            }

            User user = new User();

            var userSpotifyProfile = _spotifyWebAPI.GetPrivateProfile();

            user.Name = userSpotifyProfile.DisplayName;
            user.Email = userSpotifyProfile.Email;

            ViewData["PieChart"] = GeneratePieChart();

            ViewData["Users"] = Models.User.SelectAll();


            using (var storage = new LocalStorage())
            {
                try
                {
                    string access = storage.Get<ExternalToken>("token").AccessToken;
                    ViewData["token"] = access;
                }
                catch
                {
                    ViewData["token"] = "Error: Please logout and login with Spotify.";
                    //throw;
                }

            }



            return View();
        }

        public IActionResult About() => View();


        [Route("Home/Statistics")]
        public IActionResult Statistics()
        {
            return View();
        }

        [Route("Home/Recommendations")]
        public IActionResult Recommendations()
        {
            return View();
        }


        public IActionResult Privacy()
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
