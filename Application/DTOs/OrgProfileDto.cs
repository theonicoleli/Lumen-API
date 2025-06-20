namespace Application.DTOs
{
    public class OrgProfileDto
    {
        public int UserId { get; set; }
        public string OrgName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string OrgWebsiteUrl { get; set; } = string.Empty;
        public DateTime OrgFoundationDate { get; set; }
        public string AdminPhone { get; set; } = string.Empty;
    }
}