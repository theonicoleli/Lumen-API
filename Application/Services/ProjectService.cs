using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IOrgRepository _orgRepository;

        public ProjectService(IProjectRepository projectRepository, IOrgRepository orgRepository)
        {
            _projectRepository = projectRepository;
            _orgRepository = orgRepository;
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;
            return MapToDto(project);
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(MapToDto);
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByOrgIdAsync(int orgId)
        {
            var projects = await _projectRepository.GetByOrgIdAsync(orgId);
            return projects.Select(MapToDto);
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto)
        {
            var org = await _orgRepository.GetByIdAsync(projectDto.OrgId);
            if (org == null)
            {
                throw new KeyNotFoundException($"Organização com ID {projectDto.OrgId} não encontrada.");
            }

            var project = new Project
            {
                Name = projectDto.Name,
                Address = projectDto.Address ?? string.Empty,
                Description = projectDto.Description ?? string.Empty,
                Image_Url = projectDto.Image_Url ?? string.Empty,
                OrgId = projectDto.OrgId
            };

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            var createdProject = await _projectRepository.GetByIdAsync(project.Id);
            return MapToDto(createdProject!);
        }

        public async Task<ProjectDto?> UpdateProjectAsync(int id, ProjectUpdateDto projectDto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            if (projectDto.Name != null) project.Name = projectDto.Name;
            if (projectDto.Address != null) project.Address = projectDto.Address;
            if (projectDto.Description != null) project.Description = projectDto.Description;
            if (projectDto.Image_Url != null) project.Image_Url = projectDto.Image_Url;

            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();

            var updatedProject = await _projectRepository.GetByIdAsync(project.Id);
            return MapToDto(updatedProject!);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return false;

            await _projectRepository.DeleteAsync(id);
            await _projectRepository.SaveChangesAsync();
            return true;
        }

        private static ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Address = project.Address,
                Description = project.Description,
                Image_Url = project.Image_Url,
                OrgId = project.OrgId,
                OrgName = project.Org?.OrgName ?? "N/A"
            };
        }
    }
}