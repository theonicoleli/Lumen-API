using Converters;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class UserDonorCreateDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;

        [JsonConverter(typeof(DateTimeCustomConverter))]
        public DateTime UserBirthDate { get; set; }
        public string UserPhone { get; set; } = string.Empty;
        public string DonorDocument { get; set; } = string.Empty;
        public string DonorLocation { get; set; } = string.Empty;
    }
}
