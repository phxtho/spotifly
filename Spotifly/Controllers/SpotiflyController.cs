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
        [Route("Spotifly")]
        [Route("Spotifly/Statistics")]
        public ActionResult Statistics()
        {
            ISpotifyWebAPI api = SpotifyAuth.FetchUserEndpoint(HttpContext.Session);
            ViewData["TopGenre"] = ChartGen.GenerateUserGenreChart(UsersTopGenres(api));

            List<FullArtist> topArtists = api.GetUsersTopArtists().Items;
            int artistCount = (topArtists.Count > 5) ? 5 : topArtists.Count;
            ViewData["TopArtists"] = topArtists.GetRange(0,artistCount);

            List<FullTrack> topTracks = api.GetUsersTopTracks().Items;
            int trackCount = (topTracks.Count > 5) ? 5 : topTracks.Count;
            ViewData["TopTracks"] = topTracks.GetRange(0, trackCount);

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

        [Route("Spotifly/Recommendations")]
        public IActionResult Recommendations()
        {
            return View();
        }

        [Route("Spotifly/ArtistDetails")]
        public ActionResult ArtistDetails(string id)
        {
            try
            {
                ISpotifyWebAPI api = SpotifyAuth.FetchUserEndpoint(HttpContext.Session);
                ViewData["Artist"] = api.GetArtist(id);
                ViewData["RecommendedArtist"] = api.GetRecommendations(artistSeed: new List<string>() { id }).Tracks;//or artist, possible break point
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