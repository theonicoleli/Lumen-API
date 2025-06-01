using Domain.Entities;

namespace Infra.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByOrgIdAsync(int orgId);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
