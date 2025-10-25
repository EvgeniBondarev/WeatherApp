using System;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class WeatherData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Precipitation { get; set; }
        public string WeatherCondition { get; set; } = string.Empty;
        public double Visibility { get; set; }
        public double CloudCover { get; set; }
        public bool IsActual { get; set; }
        public string Region { get; set; } = string.Empty;
    }

    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime ForecastDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public double PredictedTemperature { get; set; }
        public double PredictedHumidity { get; set; }
        public double PredictedPressure { get; set; }
        public double PredictedWindSpeed { get; set; }
        public string PredictedWindDirection { get; set; } = string.Empty;
        public double PredictedPrecipitation { get; set; }
        public string PredictedWeatherCondition { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public string Region { get; set; } = string.Empty;
        public bool IsUpdated { get; set; }
    }

    public class ClimateData
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }
        public double AveragePrecipitation { get; set; }
        public double AverageWindSpeed { get; set; }
        public string Region { get; set; } = string.Empty;
    }

    public class AerologicalData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public double Altitude { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
