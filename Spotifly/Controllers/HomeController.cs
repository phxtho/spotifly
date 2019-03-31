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
            if (HttpContext.Session.GetString("userId") == null)
            {
                Response.Redirect("/Home/Login");
                return View("Login");
            }
            ISpotifyWebAPI api = SpotifyAuth.FetchUserEndpoint(HttpContext.Session);
            var userSpotifyProfile = api.GetPrivateProfile();

            User user = new User();
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

        [Route("Home/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Login")]
        public IActionResult PostLogin()
        {
            IFormCollection form = HttpContext.Request.Form;

            if (SpotifyAuth.LogInUser(form["email"], form["password"], HttpContext.Session))
            {
                Response.Redirect("/Home");
                Index();
                return View("Index");
            }
            Response.Redirect("/Home/Login");
            return View();
        }

        [Route("Home/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Register")]
        public IActionResult PostRegistration()
        {
            IFormCollection form = Request.Form;

            Console.WriteLine($"{form["name"]}, {form["email"]}, {form["password1"]}, {form["password2"]}");
            if (SpotifyAuth.RegisterUser(form["name"], form["email"], form["password1"], form["password2"], DateTime.Now, HttpContext.Session))
            {
                Response.Redirect("/Home");
                Index();
                return View("Index");
            }
            Response.Redirect("/Home/Registration");
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
