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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound($"Projeto com ID {id} não encontrado.");
            }
            return Ok(project);
        }

        [HttpGet("org/{orgId}")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectsByOrgId(int orgId)
        {
            var projects = await _projectService.GetProjectsByOrgIdAsync(orgId);
            if (!projects.Any())
            {
                return NotFound($"Nenhum projeto encontrado para a Organização com ID {orgId}.");
            }
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] ProjectCreateDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdProject = await _projectService.CreateProjectAsync(projectDto);
                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, [FromBody] ProjectUpdateDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedProject = await _projectService.UpdateProjectAsync(id, projectDto);
            if (updatedProject == null)
            {
                return NotFound($"Projeto com ID {id} não encontrado para atualização.");
            }
            return Ok(updatedProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var success = await _projectService.DeleteProjectAsync(id);
            if (!success)
            {
                return NotFound($"Projeto com ID {id} não encontrado para exclusão.");
            }
            return NoContent();
        }
    }
}