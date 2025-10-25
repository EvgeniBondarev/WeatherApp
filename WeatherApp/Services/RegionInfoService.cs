using System.Collections.Generic;
using System.Linq;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    /// <summary>
    /// Сервис для работы с информацией о регионах
    /// </summary>
    public class RegionInfoService
    {
        private readonly List<RegionInfo> _regions;

        public RegionInfoService()
        {
            _regions = InitializeRegions();
        }

        /// <summary>
        /// Получает информацию о всех регионах
        /// </summary>
        /// <returns>Список информации о регионах</returns>
        public List<RegionInfo> GetAllRegions()
        {
            return _regions.ToList();
        }

        /// <summary>
        /// Получает информацию о конкретном регионе
        /// </summary>
        /// <param name="regionType">Тип региона</param>
        /// <returns>Информация о регионе</returns>
        public RegionInfo GetRegionInfo(RegionType regionType)
        {
            return _regions.FirstOrDefault(r => r.Type == regionType) ?? new RegionInfo();
        }

        /// <summary>
        /// Получает русское название региона
        /// </summary>
        /// <param name="regionType">Тип региона</param>
        /// <returns>Русское название региона</returns>
        public string GetRegionName(RegionType regionType)
        {
            return GetRegionInfo(regionType).Name;
        }

        /// <summary>
        /// Получает описание региона
        /// </summary>
        /// <param name="regionType">Тип региона</param>
        /// <returns>Описание региона</returns>
        public string GetRegionDescription(RegionType regionType)
        {
            return GetRegionInfo(regionType).Description;
        }

        /// <summary>
        /// Инициализирует информацию о регионах
        /// </summary>
        /// <returns>Список регионов с подробной информацией</returns>
        private List<RegionInfo> InitializeRegions()
        {
            return new List<RegionInfo>
            {
                new RegionInfo
                {
                    Type = RegionType.Middle,
                    Name = "Срединный регион",
                    Description = "Умеренный климат с характерными сезонными изменениями",
                    Characteristics = "• Высота: 0-500 м над уровнем моря\n" +
                                   "• Температурный диапазон: 25°C\n" +
                                   "• Влажность: 30-80%\n" +
                                   "• Давление: 980-1030 гПа\n" +
                                   "• Стабильные погодные условия",
                    TypicalWeather = "• Ясная погода\n" +
                                   "• Переменная облачность\n" +
                                   "• Умеренные осадки\n" +
                                   "• Слабый и умеренный ветер",
                    Recommendations = "• Подходит для всех видов деятельности\n" +
                                    "• Стандартные меры предосторожности\n" +
                                    "• Учитывать сезонные изменения"
                },
                new RegionInfo
                {
                    Type = RegionType.Lower,
                    Name = "Нижний регион",
                    Description = "Прибрежный климат с повышенной влажностью и частыми осадками",
                    Characteristics = "• Высота: 0-200 м над уровнем моря\n" +
                                   "• Температурный диапазон: 20°C\n" +
                                   "• Влажность: 40-90%\n" +
                                   "• Давление: 990-1040 гПа\n" +
                                   "• Влияние морского климата",
                    TypicalWeather = "• Облачная погода\n" +
                                   "• Частые туманы\n" +
                                   "• Обильные осадки\n" +
                                   "• Сильные ветра с моря",
                    Recommendations = "• Осторожность при туманах\n" +
                                    "• Защита от повышенной влажности\n" +
                                    "• Учет морских бризов\n" +
                                    "• Подготовка к частым осадкам"
                },
                new RegionInfo
                {
                    Type = RegionType.Upper,
                    Name = "Верхний регион",
                    Description = "Горный климат с пониженным давлением и сильными ветрами",
                    Characteristics = "• Высота: 500-2000 м над уровнем моря\n" +
                                   "• Температурный диапазон: 30°C\n" +
                                   "• Влажность: 20-70%\n" +
                                   "• Давление: 950-1020 гПа\n" +
                                   "• Резкие перепады температуры",
                    TypicalWeather = "• Ясная погода\n" +
                                   "• Переменная облачность\n" +
                                   "• Снег и метели\n" +
                                   "• Сильные и порывистые ветра",
                    Recommendations = "• Осторожность при сильном ветре\n" +
                                    "• Защита от перепадов давления\n" +
                                    "• Подготовка к резким изменениям погоды\n" +
                                    "• Учет горного климата"
                }
            };
        }
    }
}
