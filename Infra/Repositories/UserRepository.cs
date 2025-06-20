using Domain.Entities;
using Infra.Data;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                                 .Include(u => u.DonorProfile)
                                 .Include(u => u.OrgProfile)
                                 .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                                 .Include(u => u.DonorProfile)
                                 .Include(u => u.OrgProfile)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public async Task UpdateAsync(User user) =>
            _context.Users.Update(user);

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password) =>
            await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == email && u.UserPassword == password);

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                                 .Include(u => u.DonorProfile)
                                 .Include(u => u.OrgProfile)
                                 .FirstOrDefaultAsync(u => u.UserEmail == email);
        }
    }

}
