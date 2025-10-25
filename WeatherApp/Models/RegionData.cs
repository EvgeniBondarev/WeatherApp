using System;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    public enum RegionType
    {
        Middle,    // Срединный регион
        Lower,     // Нижний регион
        Upper      // Верхний регион
    }

    public class RegionInfo
    {
        public RegionType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Characteristics { get; set; } = string.Empty;
        public string TypicalWeather { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
    }

    public class RegionCharacteristics
    {
        public RegionType RegionType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double MinAltitude { get; set; }
        public double MaxAltitude { get; set; }
        public double TypicalTemperatureRange { get; set; }
        public double TypicalHumidityRange { get; set; }
        public List<string> TypicalWeatherConditions { get; set; } = new List<string>();
    }

    public class RegionalWeatherAnalysis
    {
        public RegionType Region { get; set; }
        public DateTime AnalysisDate { get; set; }
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }
        public double AveragePressure { get; set; }
        public string DominantWindDirection { get; set; } = string.Empty;
        public double AverageWindSpeed { get; set; }
        public string WeatherPattern { get; set; } = string.Empty;
        public string ForecastReliability { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new List<string>();
    }
}
