using Domain.Entities;

namespace Infra.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task<User> GetByEmailAndPasswordAsync(string email, string password);
    }
}
