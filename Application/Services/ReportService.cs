using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            return reports.Select(r => new ReportDto
            {
                ReportId = r.ReportId,
                ReportDate = r.ReportDate,
                ReportContent = r.ReportContent
            });
        }

        public async Task<ReportDto> GetReportByIdAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null) return null;
            return new ReportDto
            {
                ReportId = report.ReportId,
                ReportDate = report.ReportDate,
                ReportContent = report.ReportContent
            };
        }

        public async Task<ReportDto> CreateReportAsync(ReportCreateDto reportDto)
        {
            var report = new Report
            {
                ReportDate = reportDto.ReportDate,
                ReportContent = reportDto.ReportContent
            };

            await _reportRepository.AddAsync(report);
            await _reportRepository.SaveChangesAsync();

            return new ReportDto
            {
                ReportId = report.ReportId,
                ReportDate = report.ReportDate,
                ReportContent = report.ReportContent
            };
        }

        public async Task<ReportDto> UpdateReportAsync(int id, ReportCreateDto reportDto)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null) return null;

            report.ReportDate = reportDto.ReportDate;
            report.ReportContent = reportDto.ReportContent;

            await _reportRepository.UpdateAsync(report);
            await _reportRepository.SaveChangesAsync();

            return new ReportDto
            {
                ReportId = report.ReportId,
                ReportDate = report.ReportDate,
                ReportContent = report.ReportContent
            };
        }

        public async Task<bool> DeleteReportAsync(int id)
        {
            await _reportRepository.DeleteAsync(id);
            await _reportRepository.SaveChangesAsync();
            return true;
        }
    }
}
