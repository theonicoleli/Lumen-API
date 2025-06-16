using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumen_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DonationsController : ControllerBase
    {
        private readonly IDonationService _donationService;
        public DonationsController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        /// <summary>
        /// Retorna todas as doações cadastradas.
        /// </summary>
        /// <returns>Lista de doações.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDto>>> GetAllDonations()
        {
            var donations = await _donationService.GetAllDonationsAsync();
            return Ok(donations);
        }

        /// <summary>
        /// Retorna os detalhes de uma doação pelo ID.
        /// </summary>
        /// <param name="id">ID da doação.</param>
        /// <returns>Objeto da doação encontrada.</returns>
        /// <response code="200">Doação encontrada.</response>
        /// <response code="404">Doação não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<DonationDto>> GetDonationById(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            if (donation == null) return NotFound();
            return Ok(donation);
        }

        /// <summary>
        /// Lista todas as doações feitas por um doador específico.
        /// </summary>
        /// <param name="donorId">ID do doador.</param>
        /// <returns>Lista de doações associadas ao doador.</returns>
        /// <response code="200">Lista encontrada.</response>
        /// <response code="404">Nenhuma doação encontrada para o doador.</response>
        [HttpGet("donors/{donorId}")]
        public async Task<IActionResult> GetByDonor(int donorId)
        {
            var list = await _donationService.GetDonationsByDonorAsync(donorId);
            if (!list.Any())
                return NotFound($"Não há doações para o donorId {donorId}.");

            return Ok(list);
        }

        /// <summary>
        /// Cria uma nova doação.
        /// </summary>
        /// <param name="donationDto">Objeto com os dados da doação.</param>
        /// <returns>Objeto da doação criada.</returns>
        /// <response code="201">Doação criada com sucesso.</response>
        [HttpPost]
        public async Task<ActionResult<DonationDto>> CreateDonation(DonationCreateDto donationDto)
        {
            var createdDonation = await _donationService.CreateDonationAsync(donationDto);
            return CreatedAtAction(nameof(GetDonationById), new { id = createdDonation.DonationId }, createdDonation);
        }

        /// <summary>
        /// Atualiza uma doação existente.
        /// </summary>
        /// <param name="id">ID da doação a ser atualizada.</param>
        /// <param name="donationDto">Novos dados da doação.</param>
        /// <returns>Objeto da doação atualizada.</returns>
        /// <response code="200">Atualização realizada com sucesso.</response>
        /// <response code="404">Doação não encontrada.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<DonationDto>> UpdateDonation(int id, DonationCreateDto donationDto)
        {
            var updatedDonation = await _donationService.UpdateDonationAsync(id, donationDto);
            if (updatedDonation == null) return NotFound();
            return Ok(updatedDonation);
        }

        /// <summary>
        /// Remove uma doação pelo ID.
        /// </summary>
        /// <param name="id">ID da doação a ser excluída.</param>
        /// <response code="204">Doação excluída com sucesso.</response>
        /// <response code="404">Doação não encontrada.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var success = await _donationService.DeleteDonationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
