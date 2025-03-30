using Domain.Entities;
using Infra.Data;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllAsync() =>
            await _context.Reports.ToListAsync();

        public async Task<Report> GetByIdAsync(int id) =>
            await _context.Reports.FindAsync(id);

        public async Task AddAsync(Report report) =>
            await _context.Reports.AddAsync(report);

        public async Task UpdateAsync(Report report) =>
            _context.Reports.Update(report);

        public async Task DeleteAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
                _context.Reports.Remove(report);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
