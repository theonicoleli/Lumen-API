using Converters;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class ReportCreateDto
    {
        [JsonConverter(typeof(DateTimeCustomConverter))]
        public DateTime ReportDate { get; set; }
        public string ReportContent { get; set; } = string.Empty;
    }
}
