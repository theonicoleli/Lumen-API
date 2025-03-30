using Application.DTOs;

namespace Application.Interfaces
{
    public interface IDonorService
    {
        Task<IEnumerable<DonorDto>> GetAllDonorsAsync();
        Task<DonorDto> GetDonorByIdAsync(int id);
        Task<DonorDto> CreateDonorAsync(DonorCreateDto donorDto);
        Task<DonorDto> UpdateDonorAsync(int id, DonorCreateDto donorDto);
        Task<bool> DeleteDonorAsync(int id);
    }
}
