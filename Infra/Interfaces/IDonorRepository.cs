﻿using Domain.Entities;

namespace Infra.Interfaces
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetAllAsync();
        Task<Donor> GetByIdAsync(int id);
        Task AddAsync(Donor donor);
        Task UpdateAsync(Donor donor);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
