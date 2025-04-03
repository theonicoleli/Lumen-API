namespace Application.DTOs
{
    public class ReportDto
    {
        public int ReportId { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReportContent { get; set; } = string.Empty;
    }
}
