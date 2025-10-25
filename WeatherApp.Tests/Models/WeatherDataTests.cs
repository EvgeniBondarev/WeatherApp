using WeatherApp.Models;
using Xunit;

namespace WeatherApp.Tests.Models
{
    public class WeatherDataTests
    {
        [Fact]
        public void WeatherData_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var weatherData = new WeatherData();

            // Assert
            Assert.Equal(0, weatherData.Id);
            Assert.Equal(default, weatherData.Date);
            Assert.Equal(string.Empty, weatherData.Location);
            Assert.Equal(0, weatherData.Temperature);
            Assert.Equal(0, weatherData.Humidity);
            Assert.Equal(0, weatherData.Pressure);
            Assert.Equal(0, weatherData.WindSpeed);
            Assert.Equal(string.Empty, weatherData.WindDirection);
            Assert.Equal(0, weatherData.Precipitation);
            Assert.Equal(string.Empty, weatherData.WeatherCondition);
            Assert.Equal(0, weatherData.Visibility);
            Assert.Equal(0, weatherData.CloudCover);
            Assert.False(weatherData.IsActual);
            Assert.Equal(string.Empty, weatherData.Region);
        }

        [Fact]
        public void WeatherData_PropertySetters_WorkCorrectly()
        {
            // Arrange
            var date = DateTime.Now;
            var weatherData = new WeatherData();

            // Act
            weatherData.Id = 1;
            weatherData.Date = date;
            weatherData.Location = "Test Location";
            weatherData.Temperature = 25.5;
            weatherData.Humidity = 65.0;
            weatherData.Pressure = 1013.25;
            weatherData.WindSpeed = 5.5;
            weatherData.WindDirection = "NE";
            weatherData.Precipitation = 0.0;
            weatherData.WeatherCondition = "Sunny";
            weatherData.Visibility = 10.0;
            weatherData.CloudCover = 20.0;
            weatherData.IsActual = true;
            weatherData.Region = "Middle";

            // Assert
            Assert.Equal(1, weatherData.Id);
            Assert.Equal(date, weatherData.Date);
            Assert.Equal("Test Location", weatherData.Location);
            Assert.Equal(25.5, weatherData.Temperature);
            Assert.Equal(65.0, weatherData.Humidity);
            Assert.Equal(1013.25, weatherData.Pressure);
            Assert.Equal(5.5, weatherData.WindSpeed);
            Assert.Equal("NE", weatherData.WindDirection);
            Assert.Equal(0.0, weatherData.Precipitation);
            Assert.Equal("Sunny", weatherData.WeatherCondition);
            Assert.Equal(10.0, weatherData.Visibility);
            Assert.Equal(20.0, weatherData.CloudCover);
            Assert.True(weatherData.IsActual);
            Assert.Equal("Middle", weatherData.Region);
        }
    }

    public class RegionInfoTests
    {
        [Fact]
        public void RegionInfo_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var regionInfo = new RegionInfo();

            // Assert
            Assert.Equal(default(RegionType), regionInfo.Type);
            Assert.Equal(string.Empty, regionInfo.Name);
            Assert.Equal(string.Empty, regionInfo.Description);
            Assert.Equal(string.Empty, regionInfo.Characteristics);
            Assert.Equal(string.Empty, regionInfo.TypicalWeather);
            Assert.Equal(string.Empty, regionInfo.Recommendations);
        }

        [Fact]
        public void RegionCharacteristics_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var regionCharacteristics = new RegionCharacteristics();

            // Assert
            Assert.Equal(default(RegionType), regionCharacteristics.RegionType);
            Assert.Equal(string.Empty, regionCharacteristics.Name);
            Assert.Equal(string.Empty, regionCharacteristics.Description);
            Assert.Equal(0, regionCharacteristics.MinAltitude);
            Assert.Equal(0, regionCharacteristics.MaxAltitude);
            Assert.Equal(0, regionCharacteristics.TypicalTemperatureRange);
            Assert.Equal(0, regionCharacteristics.TypicalHumidityRange);
            Assert.NotNull(regionCharacteristics.TypicalWeatherConditions);
            Assert.Empty(regionCharacteristics.TypicalWeatherConditions);
        }
    }
}