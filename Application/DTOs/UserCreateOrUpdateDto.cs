using Converters;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class UserCreateOrUpdateDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;

        [JsonConverter(typeof(DateTimeCustomConverter))]
        public DateTime UserBirthDate { get; set; }
        public string UserPhone { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}
