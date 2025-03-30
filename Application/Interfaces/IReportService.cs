using Application.DTOs;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        Task<ReportDto> GetReportByIdAsync(int id);
        Task<ReportDto> CreateReportAsync(ReportCreateDto reportDto);
        Task<ReportDto> UpdateReportAsync(int id, ReportCreateDto reportDto);
        Task<bool> DeleteReportAsync(int id);
    }
}
