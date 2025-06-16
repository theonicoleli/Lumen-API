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

        /// <summary>
        /// Retorna todos os projetos cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de projetos.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        /// <summary>
        /// Retorna os detalhes de um projeto específico pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <returns>Objeto contendo os dados do projeto.</returns>
        /// <response code="200">Projeto encontrado.</response>
        /// <response code="404">Projeto não encontrado.</response>
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

        /// <summary>
        /// Retorna todos os projetos vinculados a uma organização específica.
        /// </summary>
        /// <param name="orgId">ID da organização.</param>
        /// <returns>Lista de projetos associados.</returns>
        /// <response code="200">Lista de projetos retornada com sucesso.</response>
        /// <response code="404">Nenhum projeto encontrado para a organização.</response>
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

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        /// <param name="projectDto">Dados do projeto a ser criado.</param>
        /// <returns>Projeto criado com sucesso.</returns>
        /// <response code="201">Projeto criado.</response>
        /// <response code="400">Erro de validação nos dados enviados.</response>
        /// <response code="404">Entidade relacionada não encontrada (ex: ONG inexistente).</response>
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

        /// <summary>
        /// Atualiza os dados de um projeto existente.
        /// </summary>
        /// <param name="id">ID do projeto a ser atualizado.</param>
        /// <param name="projectDto">Novos dados do projeto.</param>
        /// <returns>Projeto atualizado.</returns>
        /// <response code="200">Projeto atualizado com sucesso.</response>
        /// <response code="400">Erro de validação nos dados enviados.</response>
        /// <response code="404">Projeto não encontrado.</response>
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

        /// <summary>
        /// Exclui um projeto pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto a ser excluído.</param>
        /// <response code="204">Projeto excluído com sucesso.</response>
        /// <response code="404">Projeto não encontrado.</response>
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