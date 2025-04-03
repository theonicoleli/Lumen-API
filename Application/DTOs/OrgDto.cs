namespace Application.DTOs
{
    public class OrgDto
    {
        public int OrgId { get; set; }
        public string OrgDescription { get; set; } = string.Empty;
        public string OrgWebsiteUrl { get; set; } = string.Empty;
        public string OrgLocation { get; set; } = string.Empty;
        public DateTime OrgFoundationDate { get; set; }
        public string AdminName { get; set; } = string.Empty;
        public string AdminPhone { get; set; } = string.Empty;
    }
}
