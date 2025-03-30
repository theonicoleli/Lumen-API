using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class UserCreateOrUpdateDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}
