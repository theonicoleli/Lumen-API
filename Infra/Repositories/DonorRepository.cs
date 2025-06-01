using Domain.Entities;
using Infra.Data;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly AppDbContext _context;

        public DonorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donor>> GetAllAsync() =>
            await _context.Donors.ToListAsync();

        public async Task<Donor> GetByIdAsync(int id) =>
            await _context.Donors.FindAsync(id);

        public async Task AddAsync(Donor donor) =>
            await _context.Donors.AddAsync(donor);

        public async Task UpdateAsync(Donor donor) =>
            _context.Donors.Update(donor);

        public async Task DeleteAsync(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor != null)
                _context.Donors.Remove(donor);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public async Task<Donor?> GetByUserIdAsync(int userId)
        {
            return await _context.Donors
                                 .Include(d => d.User)
                                 .FirstOrDefaultAsync(d => d.UserId == userId);
        }
    }
}
