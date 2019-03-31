using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using ChartJSCore.Models;
using ChartJSCore.Models.Bar;

namespace Spotifly.Controllers
{
    public class SpotiflyController : Controller
    {
        private ISpotifyWebAPI spotifyWebAPI;

        public SpotiflyController(ISpotifyWebAPI spotifyWebAPI_instance)
        {
            this.spotifyWebAPI = spotifyWebAPI_instance;
        }


        // GET: Spotifly
        public ActionResult Index()
        {
            ViewData["TopGenre"] = GenerateUserGenreChart(UsersTopGenres());
            ViewData["TopArtists"] = spotifyWebAPI.GetUsersTopArtists().Items.GetRange(0,5);
            ViewData["TopTracks"] = spotifyWebAPI.GetUsersTopTracks().Items.GetRange(0, 10);

            return View();
        }

        [Route("Spotifly/TrackDetails")]
        public ActionResult TrackDetails(string id)
        {
            try
            {
                ViewData["Track"] = spotifyWebAPI.GetTrack(id);
                ViewData["RecommendedTracks"] = spotifyWebAPI.GetRecommendations(trackSeed: new List<string>() { id }).Tracks;

                ViewData["RadarPlot"] = GenerateAudioFeaturesChart(spotifyWebAPI.GetAudioFeatures(id));

                return View();
            }
            catch
            {
                return RedirectToAction();
            }
           
        }

        #region PrivateMethods

        public Dictionary<string, int> UsersTopGenres()
        {
            //Get Users Top Artists
            List<FullArtist> topArtists = spotifyWebAPI.GetUsersTopArtists().Items;

            //Get the genre
            Dictionary<string, int> usersTopGenres = new Dictionary<string, int>();
            topArtists.ForEach(artist => artist.Genres.ForEach(genre => {

                //Check if this genre already occures
                if (usersTopGenres.ContainsKey(genre))
                {
                    usersTopGenres[genre]++;
                }
                else
                {
                    usersTopGenres.Add(genre, 1);
                }

            })
            );

            //Order the list
            Dictionary<string, int> orderedGenres = usersTopGenres.OrderByDescending(genre => genre.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            return orderedGenres;
        }

        public Chart GenerateUserGenreChart(Dictionary<string, int> genres)
        {
            Chart chart = new Chart();
            chart.Type = "bar";

            Data data = new Data();
            data.Labels = genres.Keys.ToList().GetRange(0,10);

            BarDataset dataset = new BarDataset()
            {
                Label = "# of occurances",
                Data = genres.Values.Select(i => (double)i).ToList().GetRange(0,10),
                BackgroundColor = new List<string>()
                {
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)",
                "rgba(255, 159, 64, 0.2)",
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)",
                "rgba(255, 159, 64, 0.2)"
                },
                BorderColor = new List<string>()
                {
                "rgba(255,99,132,1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 159, 64, 1)",
                "rgba(255,99,132,1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 159, 64, 1)"
                },
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            BarOptions options = new BarOptions()
            {
                Scales = new Scales(),
                BarPercentage = 0.7
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Scale>()
                {
                    new CartesianScale()
                    {
                        Ticks = new CartesianLinearTick()
                        {
                            BeginAtZero = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout()
            {
                Padding = new Padding()
                {
                    PaddingObject = new PaddingObject()
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }

        public Chart GenerateAudioFeaturesChart(AudioFeatures features)
        {
            Chart chart = new Chart();
            chart.Type = "radar";

            Data data = new Data();
            data.Labels = new List<string>() { "Acousticness", "Instrumentalness", "Speechness", "Danceability", "Energy", "Liveness", "Valence" };

            RadarDataset dataset1 = new RadarDataset()
            {
                Label = "Track " + features.Id,
                BackgroundColor = "rgba(179,181,198,0.2)",
                BorderColor = "rgba(179,181,198,1)",
                PointBackgroundColor = new List<string>() { "rgba(179,181,198,1)" },
                PointBorderColor = new List<string>() { "#fff" },
                PointHoverBackgroundColor = new List<string>() { "#fff" },
                PointHoverBorderColor = new List<string>() { "rgba(179,181,198,1)" },
                Data = new List<double>() { features.Acousticness, features.Instrumentalness, features.Speechiness, features.Danceability, features.Energy, features.Liveness, features.Valence }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset1);

            chart.Data = data;
            return chart;
        }

        #endregion
    }
}