using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        [Route("Home/Index")]
        [Route("Home/")]
        [Route("")]
        public IActionResult Index()
        {
            Token token = null;
            ISpotifyWebAPI api = null;

            api = SpotifyAuth.FetchUserEndpoint(HttpContext.Session);
            Console.WriteLine("-- Session data --");
            Console.WriteLine(HttpContext.Session.GetString("Example"));
            Console.WriteLine(api.GetPrivateProfile().Id);
            Console.WriteLine("-- Session data --");
            
           
            User user = new User();

            var userSpotifyProfile = _spotifyWebAPI.GetPrivateProfile();

            user.Name = userSpotifyProfile.DisplayName;
            user.Email = userSpotifyProfile.Email;

            ViewData["PieChart"] = GeneratePieChart();

            ViewData["Users"] = Models.User.SelectAll();
            return View();
        }

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
