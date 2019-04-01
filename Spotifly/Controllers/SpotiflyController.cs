using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotifly.Models;

namespace Spotifly.Controllers
{
    public class SpotiflyController : Controller
    {
        public SpotiflyController(ISpotifyWebAPI spotifyWebAPI_instance)
        {
        }


        // GET: Spotifly
        public ActionResult Index()
        {
            ISpotifyWebAPI api = SpotifyAuth.FetchUserEndpoint(HttpContext.Session);
            ViewData["TopGenre"] = ChartGen.GenerateUserGenreChart(UsersTopGenres(api));
            ViewData["TopArtists"] = api.GetUsersTopArtists().Items.GetRange(0, 5);
            ViewData["TopTracks"] = api.GetUsersTopTracks().Items.GetRange(0, 5);

            return View();
        }

        [Route("Spotifly/TrackDetails")]
        public ActionResult TrackDetails(string id)
        {
            try
            {
                ISpotifyWebAPI api = SpotifyAuth.FetchUserEndpoint(HttpContext.Session);
                ViewData["Track"] = api.GetTrack(id);
                ViewData["RecommendedTracks"] = api.GetRecommendations(trackSeed: new List<string>() { id }).Tracks;
                ViewData["SpotifyWebAPI"] = api;
                ViewData["RadarPlot"] = ChartGen.GenerateAudioFeaturesChart(api.GetAudioFeatures(id));

                return View();
            }
            catch
            {
                return RedirectToAction();
            }
           
        }

        #region PrivateMethods

        public Dictionary<string, int> UsersTopGenres(ISpotifyWebAPI api)
        {
            //Get Users Top Artists
            List<FullArtist> topArtists = api.GetUsersTopArtists().Items;

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

       

        #endregion
    }
}