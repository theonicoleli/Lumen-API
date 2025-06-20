namespace Application.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image_Url { get; set; } = string.Empty;
        public int OrgId { get; set; }
        public string OrgName { get; set; } = string.Empty;
    }
}