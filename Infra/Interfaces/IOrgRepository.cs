using Domain.Entities;

namespace Infra.Interfaces
{
    public interface IOrgRepository
    {
        Task<IEnumerable<Domain.Entities.Org>> GetAllAsync();
        Task<Domain.Entities.Org> GetByIdAsync(int id);
        Task AddAsync(Domain.Entities.Org org);
        Task UpdateAsync(Domain.Entities.Org org);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
