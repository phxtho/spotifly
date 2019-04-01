using System;
using System.Collections.Generic;
using Xunit;
using Spotifly.Models;
using SpotifyAPI.Web.Models;
using ChartJSCore.Models;

namespace Spotifly.Tests
{
    public class ChartGen_Should
    {
        [Fact]
        public void GenerateAudioFeatures_ShouldNotBeNull()
        {
            //Arrange
            AudioFeatures features = new AudioFeatures { Acousticness = 0, Danceability = 3, Energy = 4, Liveness = 5, Speechiness = 2, Valence = 6, Instrumentalness = 1} ;

            //Act
            Chart chart = ChartGen.GenerateAudioFeaturesChart(features);

            //Assert
            int count = 0;
            foreach (double data in chart.Data.Datasets[0].Data)
            {
                Assert.Equal(data, count);
                count++;
            }
            
        }

        [Fact]
        public void GenerateUserGenre_ShouldNotBeNull()
        {
            //Arrange
            Dictionary<string, int> genres = new Dictionary<string, int>();
            genres.Add("Rock", 1);

            //Act
            Chart chart = ChartGen.GenerateUserGenreChart(genres);

            //Assert
            Assert.Equal("Rock", chart.Data.Labels[0]);

        }
    }
}
