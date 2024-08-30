using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleTestJson
{
    //[JsonDerivedType(typeof(WeatherForecastBase), typeDiscriminator: "base")]
    //[JsonDerivedType(typeof(WeatherForecastWithCity), typeDiscriminator: "withCity")]
    [DerivedType(typeof(WeatherForecastBase), "base")]
    public class WeatherForecastBase
    {
        [JsonGuidParser]
        public Guid Id { get; set; }
        [JsonDateTimeOffsetParser]
        public DateTimeOffset Date { get; set; }
        [JsonIntParser]
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }

        protected SomeService? someService { get; set; } = null;

        public override string ToString()
        {
            return @$"Id: {Id}
Date: {Date.ToString("yyyy-MM-dd HH:mm:ss zzz")}
TemperatureCelsius: {TemperatureCelsius}
Summary: {Summary}";
        }
    }

    [DerivedType(typeof(WeatherForecastWithCity), "WithCity")]
    public class WeatherForecastWithCity : WeatherForecastBase
    {
        public string? City { get; set; }

        public WeatherForecastWithCity(SomeService? someService)
        {
            this.someService = someService;
        }
        public override string ToString()
        {
            return @$"City: {City}{Environment.NewLine}" + base.ToString();
        }
    }
}
