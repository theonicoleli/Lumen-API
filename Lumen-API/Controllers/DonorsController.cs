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
    public class DonorsController : ControllerBase
    {
        private readonly IDonorService _donorService;
        public DonorsController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonorDto>>> GetAllDonors()
        {
            var donors = await _donorService.GetAllDonorsAsync();
            return Ok(donors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DonorDto>> GetDonorById(int id)
        {
            var donor = await _donorService.GetDonorByIdAsync(id);
            if (donor == null) return NotFound();
            return Ok(donor);
        }

        [HttpPost]
        public async Task<ActionResult<DonorDto>> CreateDonor(DonorCreateDto donorDto)
        {
            var createdDonor = await _donorService.CreateDonorAsync(donorDto);
            return CreatedAtAction(nameof(GetDonorById), new { id = createdDonor.DonorId }, createdDonor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DonorDto>> UpdateDonor(int id, DonorCreateDto donorDto)
        {
            var updatedDonor = await _donorService.UpdateDonorAsync(id, donorDto);
            if (updatedDonor == null) return NotFound();
            return Ok(updatedDonor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            var success = await _donorService.DeleteDonorAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
