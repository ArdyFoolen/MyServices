using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ConsoleTestJson
{
    public class WeatherForecastWithCityConverter : JsonConverter<WeatherForecastBase>
    {
        private SomeService? _someService;
        private JsonParserFactory _parserFactory = new JsonParserFactory();
        public WeatherForecastWithCityConverter(SomeService? someService)
        {
            _someService = someService;
        }

        public override WeatherForecastBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var forecast = Create(ref reader);
            if (forecast == null)
                throw new JsonException("Descriptor not valid");

            while (reader.TokenType != JsonTokenType.EndObject)
                SetProperty(ref reader, forecast);

            return forecast;
        }

        public override void Write(Utf8JsonWriter writer, WeatherForecastBase value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteTypeDescriptor(value);

            writer.WriteString(nameof(value.Id), value.Id.ToString("D"));
            writer.WriteString(nameof(value.Date), value.Date.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.CurrentCulture));
            writer.WriteNumber(nameof(value.TemperatureCelsius), value.TemperatureCelsius);
            writer.WriteString(nameof(value.Summary), value.Summary);

            var withCity = value as WeatherForecastWithCity;
            if (withCity != null)
                writer.WriteString(nameof(withCity.City), withCity.City);

            writer.WriteEndObject();
        }

        private WeatherForecastBase? Create(ref Utf8JsonReader reader)
        {
            string? propertyName = ReadPropertyName(ref reader);
            if (propertyName != "$type")
                throw new JsonException();

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();
            string? descriptor = reader.GetString();

            var attribute = typeof(WeatherForecastBase).GetDerivedTypeAttributeFrom(descriptor);
            return attribute?.Create<WeatherForecastBase>(_someService);
        }

        private void SetProperty<T>(ref Utf8JsonReader reader, T foreCast)
        {
            string? propertyName = ReadPropertyName(ref reader);
            if (reader.TokenType == JsonTokenType.EndObject)
                return;

            var parser = _parserFactory.GetParser(foreCast, propertyName);
            parser?.ParseAndSet(ref reader, foreCast, propertyName);
        }

        private string? ReadPropertyName(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType == JsonTokenType.EndObject)
                return null;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            return reader.GetString();
        }
    }
}
