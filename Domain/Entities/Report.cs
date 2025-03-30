namespace Domain.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public long ReportDate { get; set; }
        public string ReportContent { get; set; } = string.Empty;
    }
}
