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
using MyServices.Extensions;
using MyServices.Commands;

namespace MyServices.Json
{
    public class NetworkCommandConverter : JsonConverter<NetworkCommand>
    {
        private IClientHandler? clientHandler = null;
        private JsonParserFactory _parserFactory = new JsonParserFactory();

        public NetworkCommandConverter() { }
        public NetworkCommandConverter(IClientHandler clientHandler)
        {
            this.clientHandler = clientHandler;
        }

        public override NetworkCommand Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var item = Create(ref reader);
            if (item == null)
                throw new JsonException("Descriptor not valid");

            while (reader.TokenType != JsonTokenType.EndObject)
                SetProperty(ref reader, item);

            return item;
        }

        public override void Write(Utf8JsonWriter writer, NetworkCommand value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteTypeDescriptor(value);

            writer.WriteString(nameof(value.SessionToken), value.SessionToken.ToString("D"));
            writer.WriteString(nameof(value.Command), value.Command);
            writer.WriteString(nameof(value.Output), value.Output);
            writer.WriteString(nameof(value.Error), value.Error);

            var outputCommand = value as OutputCommand;
            if (outputCommand != null)
                writer.WriteNumber(nameof(outputCommand.CommandEnum), (int)outputCommand.CommandEnum);

            writer.WriteEndObject();
        }

        private NetworkCommand? Create(ref Utf8JsonReader reader)
        {
            string? propertyName = ReadPropertyName(ref reader);
            if (propertyName != "$type")
                throw new JsonException();

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();
            string? descriptor = reader.GetString();

            return Create(descriptor);
        }

        private NetworkCommand? Create(string? descriptor)
            => typeof(NetworkCommand)
                .GetDerivedTypeAttributeFrom(descriptor)?
                .Create<NetworkCommand>(clientHandler);

        private void SetProperty<T>(ref Utf8JsonReader reader, T item)
        {
            string? propertyName = ReadPropertyName(ref reader);
            if (reader.TokenType == JsonTokenType.EndObject)
                return;

            var parser = _parserFactory.GetParser(item, propertyName);
            parser?.ParseAndSet(ref reader, item, propertyName);
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
