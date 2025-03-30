using Domain.Entities;

namespace Infra.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(int id);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
