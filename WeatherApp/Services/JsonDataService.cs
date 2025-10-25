using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class JsonDataService
    {
        private readonly string _dataDirectory;
        private readonly string _weatherDataFile;
        private readonly string _forecastDataFile;
        private readonly string _climateDataFile;
        private readonly string _aerologicalDataFile;
        private readonly string _regionDataFile;

        public JsonDataService()
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _weatherDataFile = Path.Combine(_dataDirectory, "weather_data.json");
            _forecastDataFile = Path.Combine(_dataDirectory, "forecast_data.json");
            _climateDataFile = Path.Combine(_dataDirectory, "climate_data.json");
            _aerologicalDataFile = Path.Combine(_dataDirectory, "aerological_data.json");
            _regionDataFile = Path.Combine(_dataDirectory, "region_data.json");

            EnsureDataDirectoryExists();
        }

        private void EnsureDataDirectoryExists()
        {
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        #region Weather Data Operations

        public async Task<List<WeatherData>> GetWeatherDataAsync()
        {
            if (!File.Exists(_weatherDataFile))
                return new List<WeatherData>();

            var json = await File.ReadAllTextAsync(_weatherDataFile);
            return JsonConvert.DeserializeObject<List<WeatherData>>(json) ?? new List<WeatherData>();
        }

        public async Task SaveWeatherDataAsync(List<WeatherData> weatherData)
        {
            var json = JsonConvert.SerializeObject(weatherData, Formatting.Indented);
            await File.WriteAllTextAsync(_weatherDataFile, json);
        }

        public async Task AddWeatherDataAsync(WeatherData weatherData)
        {
            var data = await GetWeatherDataAsync();
            weatherData.Id = data.Count > 0 ? data.Max(x => x.Id) + 1 : 1;
            data.Add(weatherData);
            await SaveWeatherDataAsync(data);
        }

        public async Task UpdateWeatherDataAsync(WeatherData weatherData)
        {
            var data = await GetWeatherDataAsync();
            var index = data.FindIndex(x => x.Id == weatherData.Id);
            if (index >= 0)
            {
                data[index] = weatherData;
                await SaveWeatherDataAsync(data);
            }
        }

        #endregion

        #region Forecast Data Operations

        public async Task<List<WeatherForecast>> GetForecastDataAsync()
        {
            if (!File.Exists(_forecastDataFile))
                return new List<WeatherForecast>();

            var json = await File.ReadAllTextAsync(_forecastDataFile);
            return JsonConvert.DeserializeObject<List<WeatherForecast>>(json) ?? new List<WeatherForecast>();
        }

        public async Task SaveForecastDataAsync(List<WeatherForecast> forecastData)
        {
            var json = JsonConvert.SerializeObject(forecastData, Formatting.Indented);
            await File.WriteAllTextAsync(_forecastDataFile, json);
        }

        public async Task AddForecastDataAsync(WeatherForecast forecast)
        {
            var data = await GetForecastDataAsync();
            forecast.Id = data.Count > 0 ? data.Max(x => x.Id) + 1 : 1;
            data.Add(forecast);
            await SaveForecastDataAsync(data);
        }

        public async Task UpdateForecastDataAsync(WeatherForecast forecast)
        {
            var data = await GetForecastDataAsync();
            var index = data.FindIndex(x => x.Id == forecast.Id);
            if (index >= 0)
            {
                data[index] = forecast;
                await SaveForecastDataAsync(data);
            }
        }

        #endregion

        #region Climate Data Operations

        public async Task<List<ClimateData>> GetClimateDataAsync()
        {
            if (!File.Exists(_climateDataFile))
                return new List<ClimateData>();

            var json = await File.ReadAllTextAsync(_climateDataFile);
            return JsonConvert.DeserializeObject<List<ClimateData>>(json) ?? new List<ClimateData>();
        }

        public async Task SaveClimateDataAsync(List<ClimateData> climateData)
        {
            var json = JsonConvert.SerializeObject(climateData, Formatting.Indented);
            await File.WriteAllTextAsync(_climateDataFile, json);
        }

        #endregion

        #region Aerological Data Operations

        public async Task<List<AerologicalData>> GetAerologicalDataAsync()
        {
            if (!File.Exists(_aerologicalDataFile))
                return new List<AerologicalData>();

            var json = await File.ReadAllTextAsync(_aerologicalDataFile);
            return JsonConvert.DeserializeObject<List<AerologicalData>>(json) ?? new List<AerologicalData>();
        }

        public async Task SaveAerologicalDataAsync(List<AerologicalData> aerologicalData)
        {
            var json = JsonConvert.SerializeObject(aerologicalData, Formatting.Indented);
            await File.WriteAllTextAsync(_aerologicalDataFile, json);
        }

        public async Task AddAerologicalDataAsync(AerologicalData aerologicalData)
        {
            var data = await GetAerologicalDataAsync();
            aerologicalData.Id = data.Count > 0 ? data.Max(x => x.Id) + 1 : 1;
            data.Add(aerologicalData);
            await SaveAerologicalDataAsync(data);
        }

        #endregion

        #region Region Data Operations

        public async Task<List<RegionCharacteristics>> GetRegionCharacteristicsAsync()
        {
            if (!File.Exists(_regionDataFile))
                return new List<RegionCharacteristics>();

            var json = await File.ReadAllTextAsync(_regionDataFile);
            return JsonConvert.DeserializeObject<List<RegionCharacteristics>>(json) ?? new List<RegionCharacteristics>();
        }

        public async Task SaveRegionCharacteristicsAsync(List<RegionCharacteristics> regionData)
        {
            var json = JsonConvert.SerializeObject(regionData, Formatting.Indented);
            await File.WriteAllTextAsync(_regionDataFile, json);
        }

        #endregion
    }
}
