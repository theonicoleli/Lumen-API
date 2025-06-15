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

        /// <summary>
        /// Retorna todos os relatórios cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de relatórios.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        /// <summary>
        /// Retorna os detalhes de um relatório específico pelo ID.
        /// </summary>
        /// <param name="id">ID do relatório.</param>
        /// <returns>Objeto contendo os dados do relatório.</returns>
        /// <response code="200">Relatório encontrado com sucesso.</response>
        /// <response code="404">Relatório não encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        /// <summary>
        /// Cria um novo relatório.
        /// </summary>
        /// <param name="reportDto">Dados do relatório a ser criado.</param>
        /// <returns>Relatório criado com sucesso.</returns>
        /// <response code="201">Relatório criado com sucesso.</response>
        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport(ReportCreateDto reportDto)
        {
            var createdReport = await _reportService.CreateReportAsync(reportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.ReportId }, createdReport);
        }

        /// <summary>
        /// Atualiza um relatório existente.
        /// </summary>
        /// <param name="id">ID do relatório a ser atualizado.</param>
        /// <param name="reportDto">Novos dados do relatório.</param>
        /// <returns>Relatório atualizado.</returns>
        /// <response code="200">Relatório atualizado com sucesso.</response>
        /// <response code="404">Relatório não encontrado.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<ReportDto>> UpdateReport(int id, ReportCreateDto reportDto)
        {
            var updatedReport = await _reportService.UpdateReportAsync(id, reportDto);
            if (updatedReport == null) return NotFound();
            return Ok(updatedReport);
        }

        /// <summary>
        /// Exclui um relatório pelo ID.
        /// </summary>
        /// <param name="id">ID do relatório a ser excluído.</param>
        /// <response code="204">Relatório excluído com sucesso.</response>
        /// <response code="404">Relatório não encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var success = await _reportService.DeleteReportAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
