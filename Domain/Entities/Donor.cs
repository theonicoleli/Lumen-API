namespace Domain.Entities
{
    public class Donor
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}
