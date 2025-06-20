using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime UserDateCreated { get; set; } 
        public virtual Donor? DonorProfile { get; set; }
        public virtual Org? OrgProfile { get; set; }
    }
}
