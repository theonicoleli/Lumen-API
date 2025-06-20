using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<IEnumerable<ProjectDto>> GetProjectsByOrgIdAsync(int orgId);
        Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto);
        Task<ProjectDto?> UpdateProjectAsync(int id, ProjectUpdateDto projectDto);
        Task<bool> DeleteProjectAsync(int id);
    }
}