using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class ProjectCreateDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(2048)]
        public string? Image_Url { get; set; }

        [Required]
        public int OrgId { get; set; }
    }
}