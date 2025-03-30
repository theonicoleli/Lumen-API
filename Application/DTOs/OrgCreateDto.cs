namespace Application.DTOs
{
    public class OrgCreateDto
    {
        public string OrgDescription { get; set; } = string.Empty;
        public string OrgWebsiteUrl { get; set; } = string.Empty;
        public string OrgLocation { get; set; } = string.Empty;
        public long OrgFoundationDate { get; set; }
    }
}
