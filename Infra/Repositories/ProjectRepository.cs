using Domain.Entities;
using Infra.Data;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                                 .Include(p => p.Org)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                                 .Include(p => p.Org)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByOrgIdAsync(int orgId)
        {
            return await _context.Projects
                                 .Where(p => p.OrgId == orgId)
                                 .Include(p => p.Org)
                                 .ToListAsync();
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var project = await GetByIdAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}