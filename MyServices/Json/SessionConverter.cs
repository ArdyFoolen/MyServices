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
using MyServices.Models;

namespace MyServices.Json
{
    public class SessionConverter : JsonConverter<Session>
    {
        private readonly EndpointConverter endpointConverter;
        public SessionConverter(EndpointConverter endpointConverter)
        {
            this.endpointConverter = endpointConverter;
        }

        public override Session Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var session = new Session();

            string? propertyName = ReadPropertyName(ref reader);

            reader.Read();
            var value = reader.GetString();
            var guid = Guid.ParseExact(value, "D");
            propertyName.SetProperty(session, guid);

            propertyName = ReadPropertyName(ref reader);
            session.EndPoint = endpointConverter.Read(ref reader, typeof(EndPoint), options);

            do
            {
                reader.Read();
            } while (reader.TokenType != JsonTokenType.EndObject);

            //reader.Read();

            return session;
        }

        public override void Write(Utf8JsonWriter writer, Session value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(value.Token), value.Token.ToString("D"));

            var endpoint = JsonSerializer.Serialize<EndPoint>(value.EndPoint, options);
            writer.WriteString(nameof(value.EndPoint), endpoint);

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
