using ChartJSCore.Models;
using ChartJSCore.Models.Bar;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotifly.Models
{
    public class ChartGen
    {
        public static Chart GenerateUserGenreChart(Dictionary<string, int> genres)
        {
            Chart chart = new Chart();
            chart.Type = "bar";

            Data data = new Data();
            data.Labels = genres.Keys.ToList().GetRange(0, genres.Keys.ToList().Count);
            int upperLimit = genres.Count > 5 ? 5 : genres.Count;
            data.Labels = genres.Keys.ToList().GetRange(0, upperLimit);

            BarDataset dataset = new BarDataset()
            {
                Label = "# of occurances",
                Data = genres.Values.Select(i => (double)i).ToList().GetRange(0, genres.Keys.ToList().Count),
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

        public static Chart GenerateAudioFeaturesChart(AudioFeatures features)
        {
            Chart chart = new Chart();
            chart.Type = "radar";

            Data data = new Data();
            data.Labels = new List<string>() { "Acousticness", "Instrumentalness", "Speechiness", "Danceability", "Energy", "Liveness", "Valence" };
            RadarDataset dataset1 = new RadarDataset()
            {
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

            RadarOptions options = new RadarOptions()
            {

                Scales = new Scales() {
                    XAxes = new List<Scale>() {
                        new RadialScale(){
                            GridLines = new GridLine(){
                                DrawBorder = false
                            }

                        }
                    },
                    YAxes = new List<Scale>() {
                        new RadialScale(){
                            GridLines = new GridLine(){
                                DrawBorder = false
                            }
                            
                        }
                    }
                }
            };

            chart.Options = options;

            return chart;
        }
    }
}
