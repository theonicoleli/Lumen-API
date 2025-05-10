using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Converters
{
    public class DateTimeCustomConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "dd/MM/yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString()!;

                if (DateTime.TryParseExact(
                        value,
                        DateFormat,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var date))
                    return date;

                if (DateTime.TryParse(
                        value,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                        out date))
                    return date;
            }

            if (reader.TokenType == JsonTokenType.StartObject || reader.TokenType == JsonTokenType.Number)
                return reader.GetDateTime();

            throw new JsonException($"Valor de data/hora inválido ou em formato não suportado.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}
