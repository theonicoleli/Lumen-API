using Application.DTOs;

namespace Application.Interfaces
{
    public interface IDonorService
    {
        Task<DonorProfileDto?> GetDonorByUserIdAsync(int userId);
        Task<IEnumerable<DonorProfileDto>> GetAllDonorsAsync();
    }
}
