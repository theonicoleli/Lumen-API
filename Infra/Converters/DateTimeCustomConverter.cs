using System;
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
            string dateString = reader.GetString();
            if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            throw new JsonException($"Não foi possível converter \"{dateString}\" para DateTime com o formato {DateFormat}.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}
