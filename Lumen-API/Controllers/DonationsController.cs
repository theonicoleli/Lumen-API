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
    public class DonationsController : ControllerBase
    {
        private readonly IDonationService _donationService;
        public DonationsController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDto>>> GetAllDonations()
        {
            var donations = await _donationService.GetAllDonationsAsync();
            return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DonationDto>> GetDonationById(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            if (donation == null) return NotFound();
            return Ok(donation);
        }

        [HttpPost]
        public async Task<ActionResult<DonationDto>> CreateDonation(DonationCreateDto donationDto)
        {
            var createdDonation = await _donationService.CreateDonationAsync(donationDto);
            return CreatedAtAction(nameof(GetDonationById), new { id = createdDonation.DonationId }, createdDonation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DonationDto>> UpdateDonation(int id, DonationCreateDto donationDto)
        {
            var updatedDonation = await _donationService.UpdateDonationAsync(id, donationDto);
            if (updatedDonation == null) return NotFound();
            return Ok(updatedDonation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var success = await _donationService.DeleteDonationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
