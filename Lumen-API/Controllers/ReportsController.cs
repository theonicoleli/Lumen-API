using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lumen_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport(ReportCreateDto reportDto)
        {
            var createdReport = await _reportService.CreateReportAsync(reportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.ReportId }, createdReport);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReportDto>> UpdateReport(int id, ReportCreateDto reportDto)
        {
            var updatedReport = await _reportService.UpdateReportAsync(id, reportDto);
            if (updatedReport == null) return NotFound();
            return Ok(updatedReport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var success = await _reportService.DeleteReportAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
