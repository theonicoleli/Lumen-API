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
    public class OrgsController : ControllerBase
    {
        private readonly IOrgService _orgService;
        public OrgsController(IOrgService orgService)
        {
            _orgService = orgService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrgDto>>> GetAllOrgs()
        {
            var orgs = await _orgService.GetAllOrgsAsync();
            return Ok(orgs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrgDto>> GetOrgById(int id)
        {
            var org = await _orgService.GetOrgByIdAsync(id);
            if (org == null) return NotFound();
            return Ok(org);
        }

        [HttpPost]
        public async Task<ActionResult<OrgDto>> CreateOrg(OrgCreateDto orgDto)
        {
            var createdOrg = await _orgService.CreateOrgAsync(orgDto);
            return CreatedAtAction(nameof(GetOrgById), new { id = createdOrg.OrgId }, createdOrg);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrgDto>> UpdateOrg(int id, OrgCreateDto orgDto)
        {
            var updatedOrg = await _orgService.UpdateOrgAsync(id, orgDto);
            if (updatedOrg == null) return NotFound();
            return Ok(updatedOrg);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrg(int id)
        {
            var success = await _orgService.DeleteOrgAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
