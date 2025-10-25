using Newtonsoft.Json;
using WeatherApp.Models;
using WeatherApp.Services;
using Xunit;

namespace WeatherApp.Tests.Services
{
    public class JsonDataServiceTests : IDisposable
    {
        private readonly string _testDataDirectory;
        private readonly JsonDataService _service;
        private readonly string _testWeatherDataFile;

        public JsonDataServiceTests()
        {
            // Используем временную директорию для тестов
            _testDataDirectory = Path.Combine(Path.GetTempPath(), $"WeatherAppTests_{Guid.NewGuid()}");
            
            // Создаем временные файлы для тестов
            _testWeatherDataFile = Path.Combine(_testDataDirectory, "weather_data.json");
            
            // Очищаем директорию перед тестом
            if (Directory.Exists(_testDataDirectory))
            {
                Directory.Delete(_testDataDirectory, true);
            }
            
            Directory.CreateDirectory(_testDataDirectory);
            
            // Создаем мок сервиса с временными путями
            _service = CreateTestJsonDataService();
        }

        private JsonDataService CreateTestJsonDataService()
        {
            // Используем рефлексию для установки путей к файлам
            var service = new JsonDataService();
            
            var field = typeof(JsonDataService).GetField("_dataDirectory", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(service, _testDataDirectory);
            
            var weatherDataField = typeof(JsonDataService).GetField("_weatherDataFile", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            weatherDataField?.SetValue(service, _testWeatherDataFile);
            
            return service;
        }

        [Fact]
        public async Task AddAndGetWeatherDataAsync_RoundTripWorksCorrectly()
        {
            // Arrange
            var testData = new WeatherData
            {
                Date = DateTime.Now,
                Location = "Test Location",
                Temperature = 25.5,
                Humidity = 65.0,
                Pressure = 1013.25,
                WindSpeed = 5.5,
                WindDirection = "NE",
                WeatherCondition = "Sunny",
                Region = "Middle"
            };

            // Act
            await _service.AddWeatherDataAsync(testData);
            var retrievedData = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(retrievedData);
            Assert.Single(retrievedData);
            var data = retrievedData[0];
            Assert.Equal("Test Location", data.Location);
            Assert.Equal(25.5, data.Temperature);
            Assert.Equal(65.0, data.Humidity);
            Assert.Equal(1013.25, data.Pressure);
            Assert.Equal(5.5, data.WindSpeed);
            Assert.Equal("NE", data.WindDirection);
            Assert.Equal("Sunny", data.WeatherCondition);
            Assert.Equal("Middle", data.Region);
        }

        [Fact]
        public async Task AddWeatherDataAsync_AutoIncrementsId()
        {
            // Arrange
            var testData1 = new WeatherData { Location = "Location 1" };
            var testData2 = new WeatherData { Location = "Location 2" };

            // Act
            await _service.AddWeatherDataAsync(testData1);
            await _service.AddWeatherDataAsync(testData2);
            var retrievedData = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(retrievedData);
            Assert.Equal(2, retrievedData.Count);
            Assert.Equal(1, retrievedData[0].Id);
            Assert.Equal(2, retrievedData[1].Id);
            Assert.Equal("Location 1", retrievedData[0].Location);
            Assert.Equal("Location 2", retrievedData[1].Location);
        }

        [Fact]
        public async Task UpdateWeatherDataAsync_WhenDataExists_UpdatesCorrectly()
        {
            // Arrange
            var originalData = new WeatherData
            {
                Date = DateTime.Now,
                Location = "Original Location",
                Temperature = 20.0,
                Region = "Middle"
            };

            await _service.AddWeatherDataAsync(originalData);
            var allData = await _service.GetWeatherDataAsync();
            var dataToUpdate = allData[0];
            dataToUpdate.Location = "Updated Location";
            dataToUpdate.Temperature = 25.0;

            // Act
            await _service.UpdateWeatherDataAsync(dataToUpdate);
            var updatedData = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(updatedData);
            Assert.Single(updatedData);
            Assert.Equal("Updated Location", updatedData[0].Location);
            Assert.Equal(25.0, updatedData[0].Temperature);
            Assert.Equal(1, updatedData[0].Id); // ID должен сохраниться
        }

        [Fact]
        public async Task UpdateWeatherDataAsync_WhenDataNotExists_DoesNothing()
        {
            // Arrange
            var nonExistentData = new WeatherData
            {
                Id = 999, // Несуществующий ID
                Location = "Non Existent",
                Temperature = 25.0
            };

            // Добавляем некоторые данные для проверки, что они не изменятся
            var existingData = new WeatherData { Location = "Existing Location" };
            await _service.AddWeatherDataAsync(existingData);

            // Act
            await _service.UpdateWeatherDataAsync(nonExistentData);
            var currentData = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(currentData);
            Assert.Single(currentData);
            Assert.Equal("Existing Location", currentData[0].Location);
            Assert.Equal(1, currentData[0].Id);
        }

        [Fact]
        public async Task GetWeatherDataAsync_WhenFileNotExists_ReturnsEmptyList()
        {
            // Act
            var result = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SaveAndGetWeatherDataAsync_PreservesAllData()
        {
            // Arrange
            var testDataList = new List<WeatherData>
            {
                new WeatherData { Location = "Location 1", Temperature = 20.0 },
                new WeatherData { Location = "Location 2", Temperature = 25.0 },
                new WeatherData { Location = "Location 3", Temperature = 30.0 }
            };

            // Act
            await _service.SaveWeatherDataAsync(testDataList);
            var retrievedData = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(retrievedData);
            Assert.Equal(3, retrievedData.Count);
            Assert.Equal("Location 1", retrievedData[0].Location);
            Assert.Equal("Location 2", retrievedData[1].Location);
            Assert.Equal("Location 3", retrievedData[2].Location);
        }

        [Fact]
        public async Task AddWeatherDataAsync_WithExistingData_AppendsCorrectly()
        {
            // Arrange
            var initialData = new List<WeatherData>
            {
                new WeatherData { Location = "Initial 1", Temperature = 15.0 },
                new WeatherData { Location = "Initial 2", Temperature = 20.0 }
            };

            await _service.SaveWeatherDataAsync(initialData);

            var newData = new WeatherData { Location = "New Location", Temperature = 25.0 };

            // Act
            await _service.AddWeatherDataAsync(newData);
            var allData = await _service.GetWeatherDataAsync();

            // Assert
            Assert.NotNull(allData);
            Assert.Equal(3, allData.Count);
            Assert.Equal("Initial 1", allData[0].Location);
            Assert.Equal("Initial 2", allData[1].Location);
            Assert.Equal("New Location", allData[2].Location);
        }

        public void Dispose()
        {
            // Cleanup test files
            if (Directory.Exists(_testDataDirectory))
            {
                try
                {
                    Directory.Delete(_testDataDirectory, true);
                }
                catch
                {
                    // Игнорируем ошибки очистки
                }
            }
        }
    }
}