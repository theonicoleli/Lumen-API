using Converters;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class OrgCreateDto
    {
        public string OrgDescription { get; set; } = string.Empty;
        public string OrgWebsiteUrl { get; set; } = string.Empty;
        public string OrgLocation { get; set; } = string.Empty;

        [JsonConverter(typeof(DateTimeCustomConverter))]
        public DateTime OrgFoundationDate { get; set; }
        public string AdminName { get; set; } = string.Empty;
        public string AdminPhone { get; set; } = string.Empty;
    }
}
