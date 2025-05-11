using Domain.Entities;
using Infra.Data;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infra.Repositories
{
    public class DonationRepository : IDonationRepository
    {
        private readonly AppDbContext _context;

        public DonationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donation>> GetAllAsync() =>
            await _context.Donations.ToListAsync();

        public async Task<Donation> GetByIdAsync(int id) =>
            await _context.Donations.FindAsync(id);

        public async Task AddAsync(Donation donation) =>
            await _context.Donations.AddAsync(donation);

        public async Task UpdateAsync(Donation donation) =>
            _context.Donations.Update(donation);

        public async Task DeleteAsync(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
                _context.Donations.Remove(donation);
        }

        public async Task<IEnumerable<Donation>> GetByDonorIdAsync(int donorId)
        {
            return await _context.Donations
                .Include(d => d.Donor)
                .Where(d => d.DonorId == donorId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
