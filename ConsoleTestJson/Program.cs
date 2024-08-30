using ConsoleTestJson;
using System.Text;
using System.Text.Json;

SomeService factory = new SomeService();
JsonSerializerOptions options = new JsonSerializerOptions();
options.Converters.Add(new WeatherForecastWithCityConverter(factory));

WeatherForecastBase weather = new WeatherForecastWithCity(factory)
{
    Id = Guid.NewGuid(),
    City = "Milwaukee",
    Date = new DateTimeOffset(2022, 9, 26, 0, 0, 0, TimeSpan.FromHours(-5)),
    TemperatureCelsius = 15,
    Summary = "Cool"
};
var json = JsonSerializer.Serialize(weather, options);
Console.WriteLine(json);
// Sample output:
//   {
//     "$type" : "withCity",
//     "City": "Milwaukee",
//     "Date": "2022-09-26 00:00:00 -05:00",
//     "TemperatureCelsius": 15,
//     "Summary": "Cool"
//   }

WeatherForecastBase value = JsonSerializer.Deserialize<WeatherForecastBase>(json, options);
Console.WriteLine($"value is WeatherForecastWithCity: {value is WeatherForecastWithCity}"); // True
Console.WriteLine($"WeatherForecast:{Environment.NewLine}{value?.ToString()}");
Console.WriteLine("---------------------------------------");

weather = new WeatherForecastBase
{
    Id = Guid.NewGuid(),
    Date = new DateTimeOffset(2024, 6, 12, 9, 41, 12, TimeSpan.FromHours(2)),
    TemperatureCelsius = 36,
    Summary = "Hot"
};
Console.WriteLine($"Date: {weather.Date.ToString()}");
json = JsonSerializer.Serialize(weather, options);
Console.WriteLine(json);
// Sample output (Utf8):
//   {
//     "$type" : "base",
//     "Date": "2024-06-12 09:41:12 \u002B02:00",
//     "TemperatureCelsius": 36,
//     "Summary": "Hot"
//   }

value = JsonSerializer.Deserialize<WeatherForecastBase>(json, options);
Console.WriteLine($"value is WeatherForecastWithCity: {value is WeatherForecastWithCity}"); // False
Console.WriteLine($"WeatherForecast:{Environment.NewLine}{value?.ToString()}");
Console.WriteLine("---------------------------------------");

WeatherForecastBase[] weatherForecastBases = new WeatherForecastBase[]
{
    new WeatherForecastWithCity(factory)
    {
        Id = Guid.NewGuid(),
        City = "Milwaukee",
        Date = new DateTimeOffset(2022, 9, 26, 0, 0, 0, TimeSpan.FromHours(-5)),
        TemperatureCelsius = 15,
        Summary = "Cool"
    },
    new WeatherForecastBase
    {
        Id = Guid.NewGuid(),
        Date = new DateTimeOffset(2024, 6, 12, 9, 41, 12, TimeSpan.FromHours(2)),
        TemperatureCelsius = 36,
        Summary = "Hot"
    }
};
json = JsonSerializer.Serialize(weatherForecastBases, options);
Console.WriteLine(json);

var array = JsonSerializer.Deserialize<WeatherForecastBase[]>(json, options);
array?.Select(a => { Console.WriteLine($"value is WeatherForecastWithCity: {a is WeatherForecastWithCity}"); Console.WriteLine($"WeatherForecast:{Environment.NewLine}{a?.ToString()}"); return a; }).ToArray();
