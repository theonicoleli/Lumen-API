namespace Application.DTOs
{
    public class ReportCreateDto
    {
        public long ReportDate { get; set; }
        public string ReportContent { get; set; } = string.Empty;
    }
}
