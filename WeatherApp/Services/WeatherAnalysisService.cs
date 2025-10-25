using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherAnalysisService
    {
        private readonly JsonDataService _dataService;

        public WeatherAnalysisService(JsonDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Анализирует погодные условия для конкретного региона
        /// </summary>
        /// <param name="region">Тип региона для анализа</param>
        /// <returns>Анализ погодных условий региона</returns>
        /// <remarks>
        /// Метод выполняет комплексный анализ погодных данных:
        /// 1. Фильтрует данные по региону
        /// 2. Вычисляет средние значения основных параметров
        /// 3. Определяет доминирующее направление ветра
        /// 4. Анализирует погодные паттерны
        /// 5. Рассчитывает надежность прогнозов
        /// 6. Генерирует рекомендации для региона
        /// </remarks>
        public async Task<RegionalWeatherAnalysis> AnalyzeRegionalWeatherAsync(RegionType region)
        {
            var weatherData = await _dataService.GetWeatherDataAsync();
            var regionData = weatherData.Where(w => w.Region == region.ToString()).ToList();

            if (!regionData.Any())
            {
                return new RegionalWeatherAnalysis
                {
                    Region = region,
                    AnalysisDate = DateTime.Now,
                    WeatherPattern = "Недостаточно данных для анализа"
                };
            }

            var analysis = new RegionalWeatherAnalysis
            {
                Region = region,
                AnalysisDate = DateTime.Now,
                AverageTemperature = regionData.Average(w => w.Temperature),
                AverageHumidity = regionData.Average(w => w.Humidity),
                AveragePressure = regionData.Average(w => w.Pressure),
                AverageWindSpeed = regionData.Average(w => w.WindSpeed),
                DominantWindDirection = GetDominantWindDirection(regionData),
                WeatherPattern = AnalyzeWeatherPattern(regionData),
                ForecastReliability = CalculateForecastReliability(regionData),
                Recommendations = GenerateRecommendations(region, regionData)
            };

            return analysis;
        }

        /// <summary>
        /// Рассчитывает прогноз погоды для региона на указанное количество дней вперед
        /// </summary>
        /// <param name="region">Тип региона</param>
        /// <param name="daysAhead">Количество дней для прогноза (по умолчанию 7)</param>
        /// <returns>Список прогнозов погоды</returns>
        /// <remarks>
        /// Алгоритм прогнозирования:
        /// 1. Берет последние 30 записей погодных данных для региона
        /// 2. Для каждого дня прогноза рассчитывает параметры на основе исторических данных
        /// 3. Использует трендовый анализ для экстраполяции значений
        /// 4. Учитывает сезонные особенности региона
        /// 5. Рассчитывает уверенность прогноза на основе стабильности данных
        /// </remarks>
        public async Task<List<WeatherForecast>> CalculateForecastAsync(RegionType region, int daysAhead = 7)
        {
            var weatherData = await _dataService.GetWeatherDataAsync();
            var regionData = weatherData.Where(w => w.Region == region.ToString()).OrderByDescending(w => w.Date).Take(30).ToList();

            var forecasts = new List<WeatherForecast>();

            for (int i = 1; i <= daysAhead; i++)
            {
                var forecastDate = DateTime.Now.AddDays(i);
                var forecast = CalculateSingleForecast(regionData, forecastDate, region);
                forecasts.Add(forecast);
            }

            return forecasts;
        }

        /// <summary>
        /// Оценивает точность прогноза путем сравнения с фактическими данными
        /// </summary>
        /// <param name="forecastId">ID прогноза для оценки</param>
        /// <returns>Точность прогноза в процентах (0-100)</returns>
        /// <remarks>
        /// Алгоритм оценки точности:
        /// 1. Находит прогноз по ID
        /// 2. Ищет фактические данные за дату прогноза и местоположение
        /// 3. Вычисляет абсолютные ошибки для основных параметров:
        ///    - Температура (°C)
        ///    - Влажность (%)
        ///    - Давление (гПа)
        /// 4. Рассчитывает среднюю ошибку
        /// 5. Конвертирует в процент точности (100 - средняя ошибка)
        /// 6. Возвращает значение в диапазоне 0-100%
        /// </remarks>
        public async Task<double> EvaluateForecastAccuracyAsync(int forecastId)
        {
            var forecasts = await _dataService.GetForecastDataAsync();
            var forecast = forecasts.FirstOrDefault(f => f.Id == forecastId);
            
            if (forecast == null) return 0;

            var weatherData = await _dataService.GetWeatherDataAsync();
            var actualData = weatherData.FirstOrDefault(w => 
                w.Date.Date == forecast.ForecastDate.Date && 
                w.Location == forecast.Location && 
                w.IsActual);

            if (actualData == null) return 0;

            var temperatureError = Math.Abs(forecast.PredictedTemperature - actualData.Temperature);
            var humidityError = Math.Abs(forecast.PredictedHumidity - actualData.Humidity);
            var pressureError = Math.Abs(forecast.PredictedPressure - actualData.Pressure);

            var accuracy = 100 - ((temperatureError + humidityError + pressureError) / 3);
            return Math.Max(0, accuracy);
        }

        /// <summary>
        /// Рассчитывает прогноз для одного дня на основе исторических данных
        /// </summary>
        /// <param name="historicalData">Исторические данные для анализа</param>
        /// <param name="forecastDate">Дата прогноза</param>
        /// <param name="region">Регион прогноза</param>
        /// <returns>Прогноз погоды на указанную дату</returns>
        /// <remarks>
        /// Алгоритм расчета прогноза:
        /// 1. Берет последние 7 записей для анализа тренда
        /// 2. Для каждого параметра рассчитывает трендовое значение
        /// 3. Определяет доминирующее направление ветра
        /// 4. Предсказывает погодные условия на основе паттернов
        /// 5. Рассчитывает уверенность прогноза
        /// </remarks>
        private WeatherForecast CalculateSingleForecast(List<WeatherData> historicalData, DateTime forecastDate, RegionType region)
        {
            if (!historicalData.Any())
            {
                // Если нет исторических данных, создаем прогноз на основе региональных характеристик
                var regionLocation = GetDefaultLocationForRegion(region);
                return new WeatherForecast
                {
                    ForecastDate = forecastDate,
                    CreatedDate = DateTime.Now,
                    Location = regionLocation,
                    PredictedTemperature = GetDefaultTemperatureForRegion(region),
                    PredictedHumidity = GetDefaultHumidityForRegion(region),
                    PredictedPressure = GetDefaultPressureForRegion(region),
                    PredictedWindSpeed = GetDefaultWindSpeedForRegion(region),
                    PredictedWindDirection = "СЗ",
                    PredictedPrecipitation = 0,
                    PredictedWeatherCondition = "Переменная облачность",
                    Confidence = 50, // Средняя уверенность для прогнозов без исторических данных
                    Region = region.ToString()
                };
            }

            // Простой алгоритм прогнозирования на основе исторических данных
            var recentData = historicalData.Take(7).ToList();
            
            var forecast = new WeatherForecast
            {
                ForecastDate = forecastDate,
                CreatedDate = DateTime.Now,
                Location = recentData.First().Location,
                PredictedTemperature = CalculateTrendValue(recentData, w => w.Temperature),
                PredictedHumidity = CalculateTrendValue(recentData, w => w.Humidity),
                PredictedPressure = CalculateTrendValue(recentData, w => w.Pressure),
                PredictedWindSpeed = CalculateTrendValue(recentData, w => w.WindSpeed),
                PredictedWindDirection = GetDominantWindDirection(recentData),
                PredictedPrecipitation = CalculateTrendValue(recentData, w => w.Precipitation),
                PredictedWeatherCondition = PredictWeatherCondition(recentData),
                Confidence = CalculateConfidence(recentData),
                Region = region.ToString()
            };

            return forecast;
        }

        /// <summary>
        /// Рассчитывает трендовое значение параметра на основе исторических данных
        /// </summary>
        /// <param name="data">Исторические данные</param>
        /// <param name="selector">Функция извлечения значения</param>
        /// <returns>Прогнозируемое значение с учетом тренда</returns>
        /// <remarks>
        /// Алгоритм расчета тренда:
        /// 1. Извлекает значения параметра из данных
        /// 2. Вычисляет линейный тренд: (последнее - первое) / количество точек
        /// 3. Прибавляет тренд к среднему значению
        /// 4. Возвращает экстраполированное значение
        /// </remarks>
        private double CalculateTrendValue(List<WeatherData> data, Func<WeatherData, double> selector)
        {
            if (data.Count < 2) return data.FirstOrDefault()?.Temperature ?? 0;

            var values = data.Select(selector).ToList();
            var trend = (values.Last() - values.First()) / values.Count;
            return values.Average() + trend;
        }

        /// <summary>
        /// Определяет доминирующее направление ветра в данных
        /// </summary>
        /// <param name="data">Погодные данные</param>
        /// <returns>Наиболее часто встречающееся направление ветра</returns>
        private string GetDominantWindDirection(List<WeatherData> data)
        {
            var directions = data.GroupBy(w => w.WindDirection)
                               .OrderByDescending(g => g.Count())
                               .FirstOrDefault()?.Key ?? "Неизвестно";
            return directions;
        }

        /// <summary>
        /// Анализирует погодный паттерн на основе средних значений параметров
        /// </summary>
        /// <param name="data">Погодные данные</param>
        /// <returns>Описание погодного паттерна</returns>
        /// <remarks>
        /// Классификация погодных паттернов:
        /// - Антициклон: давление > 1013 гПа, влажность < 60%
        /// - Циклон: давление < 1013 гПа, влажность > 70%
        /// - Переменная погода: промежуточные значения
        /// </remarks>
        private string AnalyzeWeatherPattern(List<WeatherData> data)
        {
            var avgTemp = data.Average(w => w.Temperature);
            var avgHumidity = data.Average(w => w.Humidity);
            var avgPressure = data.Average(w => w.Pressure);

            if (avgPressure > 1013 && avgHumidity < 60)
                return "Антициклон - ясная погода";
            else if (avgPressure < 1013 && avgHumidity > 70)
                return "Циклон - облачная погода с осадками";
            else
                return "Переменная погода";
        }

        /// <summary>
        /// Рассчитывает надежность прогноза на основе стабильности данных
        /// </summary>
        /// <param name="data">Погодные данные</param>
        /// <returns>Уровень надежности: "Высокая", "Средняя", "Низкая"</returns>
        /// <remarks>
        /// Критерии надежности:
        /// - Высокая: дисперсия температуры < 5
        /// - Средняя: дисперсия температуры 5-15
        /// - Низкая: дисперсия температуры > 15
        /// </remarks>
        private string CalculateForecastReliability(List<WeatherData> data)
        {
            var variance = CalculateVariance(data.Select(w => w.Temperature));
            if (variance < 5) return "Высокая";
            else if (variance < 15) return "Средняя";
            else return "Низкая";
        }

        /// <summary>
        /// Вычисляет стандартное отклонение (квадратный корень из дисперсии)
        /// </summary>
        /// <param name="values">Набор значений</param>
        /// <returns>Стандартное отклонение</returns>
        private double CalculateVariance(IEnumerable<double> values)
        {
            var valueList = values.ToList();
            var mean = valueList.Average();
            var variance = valueList.Sum(v => Math.Pow(v - mean, 2)) / valueList.Count;
            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Рассчитывает уверенность прогноза в процентах
        /// </summary>
        /// <param name="data">Погодные данные</param>
        /// <returns>Уверенность в процентах (0-100)</returns>
        /// <remarks>
        /// Уверенность обратно пропорциональна дисперсии данных:
        /// Чем больше разброс значений, тем меньше уверенность
        /// </remarks>
        private double CalculateConfidence(List<WeatherData> data)
        {
            var tempVariance = CalculateVariance(data.Select(w => w.Temperature));
            var humidityVariance = CalculateVariance(data.Select(w => w.Humidity));
            
            var avgVariance = (tempVariance + humidityVariance) / 2;
            return Math.Max(0, Math.Min(100, 100 - avgVariance));
        }

        /// <summary>
        /// Предсказывает погодные условия на основе последних наблюдений
        /// </summary>
        /// <param name="data">Погодные данные</param>
        /// <returns>Прогнозируемые погодные условия</returns>
        private string PredictWeatherCondition(List<WeatherData> data)
        {
            var recentConditions = data.Take(3).Select(w => w.WeatherCondition).ToList();
            var mostCommon = recentConditions.GroupBy(c => c)
                                           .OrderByDescending(g => g.Count())
                                           .FirstOrDefault()?.Key ?? "Переменная облачность";
            return mostCommon;
        }

        /// <summary>
        /// Получает местоположение по умолчанию для региона
        /// </summary>
        /// <param name="region">Тип региона</param>
        /// <returns>Название местоположения</returns>
        private string GetDefaultLocationForRegion(RegionType region)
        {
            return region switch
            {
                RegionType.Middle => "Москва",
                RegionType.Lower => "Санкт-Петербург", 
                RegionType.Upper => "Екатеринбург",
                _ => "Неизвестно"
            };
        }

        /// <summary>
        /// Получает температуру по умолчанию для региона
        /// </summary>
        /// <param name="region">Тип региона</param>
        /// <returns>Температура в градусах Цельсия</returns>
        private double GetDefaultTemperatureForRegion(RegionType region)
        {
            return region switch
            {
                RegionType.Middle => -5.0,
                RegionType.Lower => -2.0,
                RegionType.Upper => -12.0,
                _ => 0.0
            };
        }

        /// <summary>
        /// Получает влажность по умолчанию для региона
        /// </summary>
        /// <param name="region">Тип региона</param>
        /// <returns>Влажность в процентах</returns>
        private double GetDefaultHumidityForRegion(RegionType region)
        {
            return region switch
            {
                RegionType.Middle => 70.0,
                RegionType.Lower => 85.0,
                RegionType.Upper => 60.0,
                _ => 70.0
            };
        }

        /// <summary>
        /// Получает давление по умолчанию для региона
        /// </summary>
        /// <param name="region">Тип региона</param>
        /// <returns>Давление в гПа</returns>
        private double GetDefaultPressureForRegion(RegionType region)
        {
            return region switch
            {
                RegionType.Middle => 1013.0,
                RegionType.Lower => 1018.0,
                RegionType.Upper => 995.0,
                _ => 1013.0
            };
        }

        /// <summary>
        /// Получает скорость ветра по умолчанию для региона
        /// </summary>
        /// <param name="region">Тип региона</param>
        /// <returns>Скорость ветра в м/с</returns>
        private double GetDefaultWindSpeedForRegion(RegionType region)
        {
            return region switch
            {
                RegionType.Middle => 3.5,
                RegionType.Lower => 4.2,
                RegionType.Upper => 6.8,
                _ => 3.5
            };
        }

        private List<string> GenerateRecommendations(RegionType region, List<WeatherData> data)
        {
            var recommendations = new List<string>();
            var avgTemp = data.Average(w => w.Temperature);
            var avgHumidity = data.Average(w => w.Humidity);
            var avgWindSpeed = data.Average(w => w.WindSpeed);

            if (avgTemp < 0)
                recommendations.Add("Осторожно! Возможны гололедица и обледенение");
            else if (avgTemp > 30)
                recommendations.Add("Высокая температура! Рекомендуется избегать длительного пребывания на солнце");

            if (avgHumidity > 80)
                recommendations.Add("Высокая влажность может влиять на самочувствие");

            if (avgWindSpeed > 10)
                recommendations.Add("Сильный ветер! Будьте осторожны при передвижении");

            // Специфичные рекомендации для регионов
            switch (region)
            {
                case RegionType.Upper:
                    recommendations.Add("Верхний регион: Учитывайте горный климат и возможные перепады давления");
                    break;
                case RegionType.Middle:
                    recommendations.Add("Срединный регион: Стандартные погодные условия");
                    break;
                case RegionType.Lower:
                    recommendations.Add("Нижний регион: Возможны туманы и повышенная влажность");
                    break;
            }

            return recommendations;
        }
    }
}
