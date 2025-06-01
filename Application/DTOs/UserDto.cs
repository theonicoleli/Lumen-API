using Domain.Entities.Enums;

namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime UserDateCreated { get; set; }
        public DonorProfileDto? DonorProfile { get; set; }
        public OrgProfileDto? OrgProfile { get; set; }
    }
}