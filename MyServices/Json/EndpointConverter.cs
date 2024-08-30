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
using System.Net;

namespace MyServices.Json
{
    public class EndpointConverter : JsonConverter<EndPoint>
    {
        public EndpointConverter() { }

        public override EndPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? propertyName = ReadPropertyName(ref reader);
            reader.Read();
            var IP = reader.GetString().Split(['.']).Select(s => byte.Parse(s)).ToArray();

            IPAddress address = new IPAddress(IP);

            propertyName = ReadPropertyName(ref reader);
            reader.Read();
            var port = int.Parse(reader.GetString());

            propertyName = ReadPropertyName(ref reader);
            reader.Read();

            reader.Read();

            return new IPEndPoint(address, port);
        }

        public override void Write(Utf8JsonWriter writer, EndPoint value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var arrayValue = value.ToString()?.Split([':']) ?? [];

            if (arrayValue.Length > 0)
            {
                writer.WriteString("IP", arrayValue[0]);
                if (arrayValue.Length > 1)
                    writer.WriteString("Port", arrayValue[1]);
                writer.WriteNumber("AddressFamily", (int)value.AddressFamily);
            }

            writer.WriteEndObject();
        }

        private string? ReadPropertyName(ref Utf8JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType == JsonTokenType.StartObject)
                reader.Read();
            if (reader.TokenType == JsonTokenType.EndObject)
                return null;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            return reader.GetString();
        }
    }
}
