namespace Domain.Entities
{
    public class Org
    {
        public int UserId { get; set; }
        public string OrgName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public virtual User User { get; set; } = null!;
        public string OrgWebsiteUrl { get; set; } = string.Empty;
        public DateTime OrgFoundationDate { get; set; }
        public string AdminPhone { get; set; } = string.Empty;
        public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
