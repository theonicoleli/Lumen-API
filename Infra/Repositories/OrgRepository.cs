using Domain.Entities;
using Infra.Data;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class OrgRepository : IOrgRepository
    {
        private readonly AppDbContext _context;

        public OrgRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Org>> GetAllAsync() =>
            await _context.Orgs.ToListAsync();

        public async Task<Domain.Entities.Org> GetByIdAsync(int id) =>
            await _context.Orgs.FindAsync(id);

        public async Task AddAsync(Domain.Entities.Org org) =>
            await _context.Orgs.AddAsync(org);

        public async Task UpdateAsync(Domain.Entities.Org org) =>
            _context.Orgs.Update(org);

        public async Task DeleteAsync(int id)
        {
            var org = await _context.Orgs.FindAsync(id);
            if (org != null)
                _context.Orgs.Remove(org);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
