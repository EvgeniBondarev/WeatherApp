using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class RegionalWeatherService
    {
        private readonly JsonDataService _dataService;
        private readonly WeatherAnalysisService _analysisService;

        public RegionalWeatherService(JsonDataService dataService, WeatherAnalysisService analysisService)
        {
            _dataService = dataService;
            _analysisService = analysisService;
        }

        #region Срединный регион (Middle Region)

        public async Task<WeatherData> ProcessMiddleRegionDataAsync(WeatherData weatherData)
        {
            weatherData.Region = "Middle";
            
            // Специфичная обработка для срединного региона
            weatherData.Temperature = AdjustTemperatureForMiddleRegion(weatherData.Temperature);
            weatherData.Humidity = AdjustHumidityForMiddleRegion(weatherData.Humidity);
            weatherData.Pressure = AdjustPressureForMiddleRegion(weatherData.Pressure);
            
            await _dataService.AddWeatherDataAsync(weatherData);
            return weatherData;
        }

        public async Task<List<WeatherForecast>> GetMiddleRegionForecastAsync(int daysAhead = 7)
        {
            return await _analysisService.CalculateForecastAsync(RegionType.Middle, daysAhead);
        }

        public async Task<RegionalWeatherAnalysis> AnalyzeMiddleRegionAsync()
        {
            return await _analysisService.AnalyzeRegionalWeatherAsync(RegionType.Middle);
        }

        private double AdjustTemperatureForMiddleRegion(double temperature)
        {
            // Срединный регион имеет умеренный климат
            return temperature;
        }

        private double AdjustHumidityForMiddleRegion(double humidity)
        {
            // Нормализация влажности для срединного региона
            return Math.Max(30, Math.Min(80, humidity));
        }

        private double AdjustPressureForMiddleRegion(double pressure)
        {
            // Стандартное атмосферное давление для срединного региона
            return Math.Max(980, Math.Min(1030, pressure));
        }

        #endregion

        #region Нижний регион (Lower Region)

        public async Task<WeatherData> ProcessLowerRegionDataAsync(WeatherData weatherData)
        {
            weatherData.Region = "Lower";
            
            // Специфичная обработка для нижнего региона
            weatherData.Temperature = AdjustTemperatureForLowerRegion(weatherData.Temperature);
            weatherData.Humidity = AdjustHumidityForLowerRegion(weatherData.Humidity);
            weatherData.Pressure = AdjustPressureForLowerRegion(weatherData.Pressure);
            weatherData.Visibility = AdjustVisibilityForLowerRegion(weatherData.Visibility);
            
            await _dataService.AddWeatherDataAsync(weatherData);
            return weatherData;
        }

        public async Task<List<WeatherForecast>> GetLowerRegionForecastAsync(int daysAhead = 7)
        {
            return await _analysisService.CalculateForecastAsync(RegionType.Lower, daysAhead);
        }

        public async Task<RegionalWeatherAnalysis> AnalyzeLowerRegionAsync()
        {
            return await _analysisService.AnalyzeRegionalWeatherAsync(RegionType.Lower);
        }

        private double AdjustTemperatureForLowerRegion(double temperature)
        {
            // Нижний регион может быть теплее из-за более низкой высоты
            return temperature + 2; // Небольшая корректировка
        }

        private double AdjustHumidityForLowerRegion(double humidity)
        {
            // Нижний регион часто имеет повышенную влажность
            return Math.Max(40, Math.Min(90, humidity + 5));
        }

        private double AdjustPressureForLowerRegion(double pressure)
        {
            // Более высокое давление в нижнем регионе
            return Math.Max(990, Math.Min(1040, pressure + 10));
        }

        private double AdjustVisibilityForLowerRegion(double visibility)
        {
            // Возможны туманы в нижнем регионе
            return Math.Max(1, visibility);
        }

        #endregion

        #region Верхний регион (Upper Region)

        public async Task<WeatherData> ProcessUpperRegionDataAsync(WeatherData weatherData)
        {
            weatherData.Region = "Upper";
            
            // Специфичная обработка для верхнего региона
            weatherData.Temperature = AdjustTemperatureForUpperRegion(weatherData.Temperature);
            weatherData.Humidity = AdjustHumidityForUpperRegion(weatherData.Humidity);
            weatherData.Pressure = AdjustPressureForUpperRegion(weatherData.Pressure);
            weatherData.WindSpeed = AdjustWindSpeedForUpperRegion(weatherData.WindSpeed);
            
            await _dataService.AddWeatherDataAsync(weatherData);
            return weatherData;
        }

        public async Task<List<WeatherForecast>> GetUpperRegionForecastAsync(int daysAhead = 7)
        {
            return await _analysisService.CalculateForecastAsync(RegionType.Upper, daysAhead);
        }

        public async Task<RegionalWeatherAnalysis> AnalyzeUpperRegionAsync()
        {
            return await _analysisService.AnalyzeRegionalWeatherAsync(RegionType.Upper);
        }

        private double AdjustTemperatureForUpperRegion(double temperature)
        {
            // Верхний регион холоднее из-за высоты
            return temperature - 5; // Корректировка на высоту
        }

        private double AdjustHumidityForUpperRegion(double humidity)
        {
            // Верхний регион может иметь пониженную влажность
            return Math.Max(20, Math.Min(70, humidity - 10));
        }

        private double AdjustPressureForUpperRegion(double pressure)
        {
            // Пониженное давление в верхнем регионе
            return Math.Max(950, Math.Min(1020, pressure - 20));
        }

        private double AdjustWindSpeedForUpperRegion(double windSpeed)
        {
            // Верхний регион часто имеет сильные ветра
            return windSpeed * 1.2;
        }

        #endregion

        #region Общие методы для всех регионов

        public async Task<Dictionary<RegionType, RegionalWeatherAnalysis>> AnalyzeAllRegionsAsync()
        {
            var results = new Dictionary<RegionType, RegionalWeatherAnalysis>();
            
            results[RegionType.Middle] = await AnalyzeMiddleRegionAsync();
            results[RegionType.Lower] = await AnalyzeLowerRegionAsync();
            results[RegionType.Upper] = await AnalyzeUpperRegionAsync();
            
            return results;
        }

        public async Task<Dictionary<RegionType, List<WeatherForecast>>> GetForecastForAllRegionsAsync(int daysAhead = 7)
        {
            var results = new Dictionary<RegionType, List<WeatherForecast>>();
            
            results[RegionType.Middle] = await GetMiddleRegionForecastAsync(daysAhead);
            results[RegionType.Lower] = await GetLowerRegionForecastAsync(daysAhead);
            results[RegionType.Upper] = await GetUpperRegionForecastAsync(daysAhead);
            
            return results;
        }

        public async Task<WeatherData> ProcessWeatherDataByRegionAsync(WeatherData weatherData, RegionType region)
        {
            return region switch
            {
                RegionType.Middle => await ProcessMiddleRegionDataAsync(weatherData),
                RegionType.Lower => await ProcessLowerRegionDataAsync(weatherData),
                RegionType.Upper => await ProcessUpperRegionDataAsync(weatherData),
                _ => throw new ArgumentException("Неизвестный тип региона")
            };
        }

        public async Task<List<WeatherData>> GetWeatherDataByRegionAsync(RegionType region)
        {
            var allData = await _dataService.GetWeatherDataAsync();
            return allData.Where(w => w.Region == region.ToString()).ToList();
        }

        public async Task<ClimateData?> GetClimateCharacteristicsAsync(RegionType region, string month)
        {
            var climateData = await _dataService.GetClimateDataAsync();
            return climateData.FirstOrDefault(c => c.Region == region.ToString() && c.Month == month);
        }

        #endregion
    }
}
