# Система мониторинга погоды

WPF приложение для мониторинга метеорологических условий с поддержкой трех регионов: Срединный, Нижний и Верхний.

## Функциональность

### Основные возможности:
- **Ввод/получение данных** мониторинга метеорологических условий
- **Запрос данных** по различным критериям
- **Анализ карт погоды** и аэрологической информации
- **Анализ климатических данных**
- **Расчет прогнозных показателей**
- **Уточнение прогнозных показателей**
- **Обновление информации**
- **Ввод фактических показателей**
- **Отправка на анализ**
- **Вывод прогноза погоды**
- **Оценка прогноза**

### Специализированные методы для регионов:

#### Срединный регион (Middle)
- Умеренный климат
- Стандартные погодные условия
- Нормализация влажности (30-80%)
- Стандартное атмосферное давление (980-1030 гПа)

#### Нижний регион (Lower)
- Прибрежный климат
- Повышенная влажность (+5%)
- Более высокое давление (+10 гПа)
- Возможны туманы
- Корректировка температуры (+2°C)

#### Верхний регион (Upper)
- Горный климат
- Пониженное давление (-20 гПа)
- Пониженная влажность (-10%)
- Сильные ветра (×1.2)
- Корректировка температуры (-5°C)

## Структура проекта

```
WeatherApp/
├── Models/                 # Модели данных
│   ├── [WeatherData.cs](Models/WeatherData.cs)     # Основные погодные данные
│   └── [RegionData.cs](Models/RegionData.cs)      # Данные регионов
├── Services/              # Сервисы
│   ├── [JsonDataService.cs](Services/JsonDataService.cs)        # Работа с JSON
│   ├── [WeatherAnalysisService.cs](Services/WeatherAnalysisService.cs) # Анализ погоды
│   ├── [RegionalWeatherService.cs](Services/RegionalWeatherService.cs) # Региональные методы
│   └── [RegionInfoService.cs](Services/RegionInfoService.cs) # Информация о регионах
├── ViewModels/            # ViewModels для MVVM
│   ├── [BaseViewModel.cs](ViewModels/BaseViewModel.cs)
│   └── [MainViewModel.cs](ViewModels/MainViewModel.cs)
├── Views/                 # XAML представления
│   ├── [MainWindow.xaml](Views/MainWindow.xaml)
│   └── [MainWindow.xaml.cs](Views/MainWindow.xaml.cs)
├── Data/                  # JSON файлы с данными
│   ├── [weather_data.json](Data/weather_data.json)
│   ├── [forecast_data.json](Data/forecast_data.json)
│   ├── [climate_data.json](Data/climate_data.json)
│   ├── [aerological_data.json](Data/aerological_data.json)
│   └── [region_data.json](Data/region_data.json)
├── [App.xaml](App.xaml)
├── [App.xaml.cs](App.xaml.cs)
└── [WeatherApp.csproj](WeatherApp.csproj)
```

## Технологии

- **.NET 6.0** - основная платформа
- **WPF** - пользовательский интерфейс
- **MVVM** - архитектурный паттерн
- **Newtonsoft.Json** - работа с JSON
- **C#** - язык программирования

## Установка и запуск

1. Убедитесь, что установлен .NET 6.0 SDK
2. Клонируйте репозиторий
3. Откройте проект в Visual Studio или используйте командную строку:

```bash
dotnet restore
dotnet build
dotnet run
```

## Использование

### Вкладка "Данные о погоде"
- Просмотр всех записей о погоде
- Добавление новых данных
- Редактирование существующих записей
- Удаление записей
- Выбор региона для обработки данных

### Вкладка "Прогнозы"
- Генерация прогнозов для всех регионов
- Просмотр существующих прогнозов
- Оценка точности прогнозов
- Выбор региона для анализа

### Вкладка "Анализ регионов"
- Анализ погодных условий по регионам
- Сравнение характеристик регионов
- Получение рекомендаций

## Формат данных

### Погодные данные ([WeatherData](Models/WeatherData.cs#L5))
- ID, дата, местоположение
- Температура, влажность, давление
- Скорость и направление ветра
- Осадки, видимость, облачность
- Погодные условия
- Флаг фактических данных
- Регион

### Прогнозы ([WeatherForecast](Models/WeatherData.cs#L25))
- ID, дата прогноза, дата создания
- Местоположение
- Прогнозируемые параметры
- Уверенность прогноза
- Регион

### Климатические данные ([ClimateData](Models/WeatherData.cs#L45))
- Местоположение, месяц
- Средние значения параметров
- Регион

### Аэрологические данные ([AerologicalData](Models/WeatherData.cs#L55))
- Дата, местоположение, высота
- Параметры на разных высотах
- Регион

### Информация о регионах ([RegionInfo](Models/RegionData.cs#L13))
- Название и описание региона
- Характеристики климата
- Типичная погода
- Рекомендации

## Алгоритмы расчетов

### 🔍 **Анализ погодных данных**
**Основной класс:** [`WeatherAnalysisService`](Services/WeatherAnalysisService.cs)  
**Метод:** [`AnalyzeRegionalWeatherAsync()`](Services/WeatherAnalysisService.cs#L32)

```csharp
// Алгоритм регионального анализа
1. Фильтрация данных по региону
2. Вычисление средних значений параметров
3. Определение доминирующего направления ветра
4. Анализ погодных паттернов (антициклон/циклон)
5. Расчет надежности прогнозов
6. Генерация рекомендаций
```

### 📊 **Прогнозирование погоды**
**Основной класс:** [`WeatherAnalysisService`](Services/WeatherAnalysisService.cs)  
**Метод:** [`CalculateForecastAsync()`](Services/WeatherAnalysisService.cs#L78)

```csharp
// Алгоритм трендового прогнозирования
1. Взятие последних 30 записей для анализа
2. Расчет линейного тренда: (последнее - первое) / количество точек
3. Экстраполяция: среднее значение + тренд
4. Определение уверенности на основе дисперсии
5. Предсказание погодных условий по паттернам
```

### 🎯 **Оценка точности прогнозов**
**Основной класс:** [`WeatherAnalysisService`](Services/WeatherAnalysisService.cs)  
**Метод:** [`EvaluateForecastAccuracyAsync()`](Services/WeatherAnalysisService.cs#L112)

```csharp
// Алгоритм оценки точности
1. Поиск фактических данных за дату прогноза
2. Вычисление абсолютных ошибок:
   - Температура (°C)
   - Влажность (%)
   - Давление (гПа)
3. Расчет средней ошибки
4. Конвертация в процент точности: 100 - средняя ошибка
```

### 📐 **Математические формулы**

#### **Трендовый анализ**
```
Тренд = (Последнее_значение - Первое_значение) / Количество_точек
Прогноз = Среднее_значение + Тренд
```

#### **Стандартное отклонение**
```
σ = √(Σ(xi - μ)² / n)
где:
- σ - стандартное отклонение
- xi - отдельные значения
- μ - среднее значение
- n - количество значений
```

#### **Уверенность прогноза**
```
Уверенность = max(0, min(100, 100 - Средняя_дисперсия))
где:
- Средняя_дисперсия = (Дисперсия_температуры + Дисперсия_влажности) / 2
```

#### **Оценка точности**
```
Точность = max(0, 100 - (|T_прогноз - T_факт| + |H_прогноз - H_факт| + |P_прогноз - P_факт|) / 3)
где:
- T - температура
- H - влажность  
- P - давление
```

### 🏔️ **Региональная обработка данных**

**Основной класс:** [`RegionalWeatherService`](Services/RegionalWeatherService.cs)

#### **Срединный регион**
**Методы:** [`ProcessMiddleRegionDataAsync()`](Services/RegionalWeatherService.cs#L25), [`GetMiddleRegionForecastAsync()`](Services/RegionalWeatherService.cs#L35), [`AnalyzeMiddleRegionAsync()`](Services/RegionalWeatherService.cs#L40)

- **Корректировка влажности**: нормализация в диапазоне 30-80%
- **Корректировка давления**: ограничение 980-1030 гПа
- **Температура**: без изменений (умеренный климат)

#### **Нижний регион**
**Методы:** [`ProcessLowerRegionDataAsync()`](Services/RegionalWeatherService.cs#L55), [`GetLowerRegionForecastAsync()`](Services/RegionalWeatherService.cs#L65), [`AnalyzeLowerRegionAsync()`](Services/RegionalWeatherService.cs#L70)

- **Корректировка температуры**: +2°C (влияние моря)
- **Корректировка влажности**: +5% (прибрежный климат)
- **Корректировка давления**: +10 гПа (низкая высота)
- **Видимость**: минимум 1 км (туманы)

#### **Верхний регион**
**Методы:** [`ProcessUpperRegionDataAsync()`](Services/RegionalWeatherService.cs#L85), [`GetUpperRegionForecastAsync()`](Services/RegionalWeatherService.cs#L95), [`AnalyzeUpperRegionAsync()`](Services/RegionalWeatherService.cs#L100)

- **Корректировка температуры**: -5°C (высота)
- **Корректировка влажности**: -10% (горный климат)
- **Корректировка давления**: -20 гПа (высота)
- **Скорость ветра**: ×1.2 (горные ветра)

### 🌤️ **Классификация погодных паттернов**

#### **Антициклон (Высокое давление)**
```
Условия: Давление > 1013 гПа И Влажность < 60%
Характеристики:
- Ясная погода
- Слабый ветер
- Хорошая видимость
- Стабильные условия
```

#### **Циклон (Низкое давление)**
```
Условия: Давление < 1013 гПа И Влажность > 70%
Характеристики:
- Облачная погода
- Осадки
- Сильный ветер
- Нестабильные условия
```

#### **Переменная погода**
```
Условия: Промежуточные значения давления и влажности
Характеристики:
- Переменная облачность
- Умеренные условия
- Средняя стабильность
```

### 📊 **Критерии надежности прогнозов**

| Уровень | Дисперсия температуры | Описание |
|---------|----------------------|----------|
| **Высокая** | < 5°C | Стабильные условия, точные прогнозы |
| **Средняя** | 5-15°C | Умеренная стабильность |
| **Низкая** | > 15°C | Нестабильные условия, неточные прогнозы |

## Основные сервисы

### 📁 **Работа с данными**
- **[JsonDataService](Services/JsonDataService.cs)** - работа с JSON файлами
  - [`GetWeatherDataAsync()`](Services/JsonDataService.cs#L25) - получение погодных данных
  - [`SaveWeatherDataAsync()`](Services/JsonDataService.cs#L35) - сохранение данных
  - [`AddWeatherDataAsync()`](Services/JsonDataService.cs#L40) - добавление новых записей
  - [`UpdateWeatherDataAsync()`](Services/JsonDataService.cs#L45) - обновление записей

### 🔬 **Анализ и прогнозирование**
- **[WeatherAnalysisService](Services/WeatherAnalysisService.cs)** - анализ погодных данных
  - [`AnalyzeRegionalWeatherAsync()`](Services/WeatherAnalysisService.cs#L32) - региональный анализ
  - [`CalculateForecastAsync()`](Services/WeatherAnalysisService.cs#L78) - расчет прогнозов
  - [`EvaluateForecastAccuracyAsync()`](Services/WeatherAnalysisService.cs#L112) - оценка точности

### 🏔️ **Региональные методы**
- **[RegionalWeatherService](Services/RegionalWeatherService.cs)** - обработка по регионам
  - [`ProcessMiddleRegionDataAsync()`](Services/RegionalWeatherService.cs#L25) - срединный регион
  - [`ProcessLowerRegionDataAsync()`](Services/RegionalWeatherService.cs#L55) - нижний регион
  - [`ProcessUpperRegionDataAsync()`](Services/RegionalWeatherService.cs#L85) - верхний регион

### ℹ️ **Информация о регионах**
- **[RegionInfoService](Services/RegionInfoService.cs)** - работа с региональной информацией
  - [`GetAllRegions()`](Services/RegionInfoService.cs#L20) - получение всех регионов
  - [`GetRegionInfo()`](Services/RegionInfoService.cs#L28) - информация о конкретном регионе

## Особенности реализации

1. **JSON хранение** - все данные хранятся в JSON файлах
2. **Региональная обработка** - специальные алгоритмы для каждого региона
3. **Прогнозирование** - трендовый анализ на основе исторических данных
4. **Анализ точности** - статистическая оценка качества прогнозов
5. **MVVM архитектура** - разделение логики и представления
6. **Русская локализация** - интерфейс и регионы на русском языке

## Архитектура приложения

### 🎯 **MVVM Pattern**
- **[BaseViewModel](ViewModels/BaseViewModel.cs)** - базовый класс с INotifyPropertyChanged
- **[MainViewModel](ViewModels/MainViewModel.cs)** - основная логика приложения
  - [`LoadDataAsync()`](ViewModels/MainViewModel.cs#L125) - загрузка данных
  - [`AddWeatherDataAsync()`](ViewModels/MainViewModel.cs#L150) - добавление данных
  - [`GenerateForecastAsync()`](ViewModels/MainViewModel.cs#L220) - генерация прогнозов
  - [`AnalyzeRegionAsync()`](ViewModels/MainViewModel.cs#L250) - анализ регионов

### 🖥️ **Пользовательский интерфейс**
- **[MainWindow.xaml](Views/MainWindow.xaml)** - главное окно приложения
- **[MainWindow.xaml.cs](Views/MainWindow.xaml.cs)** - код-behind главного окна
- **[App.xaml](App.xaml)** - конфигурация приложения
- **[App.xaml.cs](App.xaml.cs)** - точка входа приложения

### 📊 **Команды и привязки**
- **[RelayCommand](ViewModels/MainViewModel.cs#L363)** - реализация ICommand для привязки
- **Data Binding** - привязка данных между View и ViewModel
- **Commands** - команды для обработки пользовательских действий

## Разработка

Для добавления новых функций:
1. Расширьте модели данных в папке [`Models/`](Models/)
2. Добавьте бизнес-логику в [`Services/`](Services/)
3. Обновите ViewModels в [`ViewModels/`](ViewModels/)
4. Измените представления в [`Views/`](Views/)

## Лицензия

Проект создан в учебных целях.
