using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly JsonDataService _dataService;
        private readonly RegionalWeatherService _regionalService;
        private readonly WeatherAnalysisService _analysisService;
        private readonly RegionInfoService _regionInfoService;

        private ObservableCollection<WeatherData> _weatherData = new();
        private ObservableCollection<WeatherForecast> _forecasts = new();
        private ObservableCollection<RegionalWeatherAnalysis> _regionalAnalyses = new();
        private WeatherData _selectedWeatherData = new() { Date = DateTime.Now };
        private WeatherForecast _selectedForecast = new();
        private RegionalWeatherAnalysis _selectedAnalysis = new();
        private string _selectedRegion = "Middle";
        private bool _isLoading;
        private string _statusMessage = "Готово";

        public MainViewModel()
        {
            _dataService = new JsonDataService();
            _analysisService = new WeatherAnalysisService(_dataService);
            _regionalService = new RegionalWeatherService(_dataService, _analysisService);
            _regionInfoService = new RegionInfoService();

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            AddWeatherDataCommand = new RelayCommand(async () => await AddWeatherDataAsync());
            UpdateWeatherDataCommand = new RelayCommand(async () => await UpdateWeatherDataAsync());
            DeleteWeatherDataCommand = new RelayCommand(async () => await DeleteWeatherDataAsync());
            GenerateForecastCommand = new RelayCommand(async () => await GenerateForecastAsync());
            AnalyzeRegionCommand = new RelayCommand(async () => await AnalyzeRegionAsync());
            EvaluateForecastCommand = new RelayCommand(async () => await EvaluateForecastAsync());
            RefreshDataCommand = new RelayCommand(async () => await RefreshDataAsync());
            ClearForecastsCommand = new RelayCommand(async () => await ClearForecastsAsync());

            // Загружаем данные при инициализации
            _ = Task.Run(async () => await LoadDataAsync());
        }

        #region Properties

        public ObservableCollection<WeatherData> WeatherData
        {
            get => _weatherData;
            set => SetProperty(ref _weatherData, value);
        }

        public ObservableCollection<WeatherForecast> Forecasts
        {
            get => _forecasts;
            set => SetProperty(ref _forecasts, value);
        }

        public ObservableCollection<RegionalWeatherAnalysis> RegionalAnalyses
        {
            get => _regionalAnalyses;
            set => SetProperty(ref _regionalAnalyses, value);
        }

        public WeatherData SelectedWeatherData
        {
            get => _selectedWeatherData;
            set => SetProperty(ref _selectedWeatherData, value);
        }

        public WeatherForecast SelectedForecast
        {
            get => _selectedForecast;
            set => SetProperty(ref _selectedForecast, value);
        }

        public RegionalWeatherAnalysis SelectedAnalysis
        {
            get => _selectedAnalysis;
            set => SetProperty(ref _selectedAnalysis, value);
        }

        public string SelectedRegion
        {
            get => _selectedRegion;
            set => SetProperty(ref _selectedRegion, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public List<string> Regions => new() { "Срединный", "Нижний", "Верхний" };

        public List<RegionInfo> RegionInfos => _regionInfoService.GetAllRegions();

        public List<string> WeatherConditions => new() 
        { 
            "Ясно", 
            "Переменная облачность", 
            "Облачно", 
            "Пасмурно", 
            "Дождь", 
            "Сильный дождь", 
            "Ливень", 
            "Снег", 
            "Сильный снег", 
            "Метель", 
            "Туман", 
            "Гроза", 
            "Град", 
            "Гололед", 
            "Пыльная буря" 
        };

        public List<string> WindDirections => new() 
        { 
            "С", "СВ", "В", "ЮВ", "Ю", "ЮЗ", "З", "СЗ" 
        };

        public List<string> CloudCoverOptions => new() 
        { 
            "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" 
        };

        public List<string> VisibilityOptions => new() 
        { 
            "0.5", "1.0", "2.0", "3.0", "4.0", "5.0", "6.0", "8.0", "10.0", "15.0", "20.0", "25.0", "30.0" 
        };

        #endregion

        #region Commands

        public ICommand LoadDataCommand { get; }
        public ICommand AddWeatherDataCommand { get; }
        public ICommand UpdateWeatherDataCommand { get; }
        public ICommand DeleteWeatherDataCommand { get; }
        public ICommand GenerateForecastCommand { get; }
        public ICommand AnalyzeRegionCommand { get; }
        public ICommand EvaluateForecastCommand { get; }
        public ICommand RefreshDataCommand { get; }
        public ICommand ClearForecastsCommand { get; }

        #endregion

        #region Command Implementations

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Загрузка данных...";

                var weatherData = await _dataService.GetWeatherDataAsync();
                var forecasts = await _dataService.GetForecastDataAsync();

                WeatherData.Clear();
                foreach (var data in weatherData)
                {
                    WeatherData.Add(data);
                }

                Forecasts.Clear();
                foreach (var forecast in forecasts)
                {
                    Forecasts.Add(forecast);
                }

                StatusMessage = $"Загружено {weatherData.Count} записей погоды и {forecasts.Count} прогнозов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddWeatherDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Добавление данных о погоде...";

                if (SelectedWeatherData == null || string.IsNullOrEmpty(SelectedWeatherData.Location))
                {
                    StatusMessage = "Пожалуйста, заполните все обязательные поля";
                    return;
                }

                var regionType = GetRegionTypeFromName(SelectedRegion);
                await _regionalService.ProcessWeatherDataByRegionAsync(SelectedWeatherData, regionType);

                await LoadDataAsync();
                StatusMessage = "Данные о погоде успешно добавлены";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка добавления: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateWeatherDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Обновление данных о погоде...";

                if (SelectedWeatherData == null || SelectedWeatherData.Id == 0)
                {
                    StatusMessage = "Пожалуйста, выберите запись для обновления";
                    return;
                }

                await _dataService.UpdateWeatherDataAsync(SelectedWeatherData);
                await LoadDataAsync();
                StatusMessage = "Данные о погоде успешно обновлены";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка обновления: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteWeatherDataAsync()
        {
            try
            {
                if (SelectedWeatherData == null || SelectedWeatherData.Id == 0)
                {
                    StatusMessage = "Пожалуйста, выберите запись для удаления";
                    return;
                }

                var allData = await _dataService.GetWeatherDataAsync();
                allData.RemoveAll(w => w.Id == SelectedWeatherData.Id);
                await _dataService.SaveWeatherDataAsync(allData);

                await LoadDataAsync();
                StatusMessage = "Данные о погоде успешно удалены";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка удаления: {ex.Message}";
            }
        }

        private async Task GenerateForecastAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Генерация прогноза...";

                var forecasts = await _regionalService.GetForecastForAllRegionsAsync(7);

                var allForecasts = new List<WeatherForecast>();
                foreach (var regionForecasts in forecasts.Values)
                {
                    allForecasts.AddRange(regionForecasts);
                }

                foreach (var forecast in allForecasts)
                {
                    await _dataService.AddForecastDataAsync(forecast);
                }

                await LoadDataAsync();
                StatusMessage = $"Сгенерировано {allForecasts.Count} прогнозов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка генерации прогноза: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AnalyzeRegionAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Анализ региональных данных...";

                var analyses = await _regionalService.AnalyzeAllRegionsAsync();

                RegionalAnalyses.Clear();
                foreach (var analysis in analyses.Values)
                {
                    RegionalAnalyses.Add(analysis);
                }

                StatusMessage = $"Проанализировано {analyses.Count} регионов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка анализа: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task EvaluateForecastAsync()
        {
            try
            {
                if (SelectedForecast == null || SelectedForecast.Id == 0)
                {
                    StatusMessage = "Пожалуйста, выберите прогноз для оценки";
                    return;
                }

                var accuracy = await _analysisService.EvaluateForecastAccuracyAsync(SelectedForecast.Id);
                StatusMessage = $"Точность прогноза: {accuracy:F1}%";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка оценки прогноза: {ex.Message}";
            }
        }

        private async Task RefreshDataAsync()
        {
            await LoadDataAsync();
        }

        private async Task ClearForecastsAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Очистка прогнозов...";

                await _dataService.SaveForecastDataAsync(new List<WeatherForecast>());
                await LoadDataAsync();
                StatusMessage = "Прогнозы очищены";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка очистки прогнозов: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Конвертирует русское название региона в тип региона
        /// </summary>
        /// <param name="regionName">Русское название региона</param>
        /// <returns>Тип региона</returns>
        private RegionType GetRegionTypeFromName(string regionName)
        {
            return regionName switch
            {
                "Срединный" => RegionType.Middle,
                "Нижний" => RegionType.Lower,
                "Верхний" => RegionType.Upper,
                _ => RegionType.Middle
            };
        }

        /// <summary>
        /// Конвертирует тип региона в русское название
        /// </summary>
        /// <param name="regionType">Тип региона</param>
        /// <returns>Русское название региона</returns>
        private string GetRegionNameFromType(RegionType regionType)
        {
            return regionType switch
            {
                RegionType.Middle => "Срединный",
                RegionType.Lower => "Нижний",
                RegionType.Upper => "Верхний",
                _ => "Срединный"
            };
        }

        #endregion
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public async void Execute(object? parameter)
        {
            await _execute();
        }
    }
}
