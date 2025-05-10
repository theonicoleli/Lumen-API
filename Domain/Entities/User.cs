namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;
        public string UserImageUrl { get; set; } = string.Empty;
        public int DonorId { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime UserDateCreated { get; set; }
        public string Phone { get; set; } = string.Empty;
    }
}
