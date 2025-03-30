using Domain.Entities;

namespace Infra.Interfaces
{
    public interface IDonationRepository
    {
        Task<IEnumerable<Donation>> GetAllAsync();
        Task<Donation> GetByIdAsync(int id);
        Task AddAsync(Donation donation);
        Task UpdateAsync(Donation donation);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
