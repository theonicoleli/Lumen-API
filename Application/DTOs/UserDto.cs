namespace Application.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;
        public string UserImageUrl { get; set; } = string.Empty;
        public int DonorId { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string UserPhone { get; set; } = string.Empty;
    }
}
