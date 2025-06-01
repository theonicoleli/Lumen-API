namespace Application.DTOs
{
    public class OrgProfileUpdateDto
    {
        public string? OrgName { get; set; }
        public string? Phone { get; set; }
        public string? Document { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? AdminName { get; set; }
        public string? OrgWebsiteUrl { get; set; }
        public DateTime? OrgFoundationDate { get; set; }
        public string? AdminPhone { get; set; }
        public bool ClearImage { get; set; } = false;
    }
}