namespace Domain.Entities
{
    public class Org
    {
        public int OrgId { get; set; }
        public string OrgDescription { get; set; } = string.Empty;
        public string OrgWebsiteUrl { get; set; } = string.Empty;
        public string OrgLocation { get; set; } = string.Empty;
        public long OrgFoundationDate { get; set; }
        public ICollection<Donation>? Donations { get; set; }
    }
}
