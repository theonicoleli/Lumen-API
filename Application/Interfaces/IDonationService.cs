using Application.DTOs;

namespace Application.Interfaces
{
    public interface IDonationService
    {
        Task<IEnumerable<DonationDto>> GetAllDonationsAsync();
        Task<DonationDto> GetDonationByIdAsync(int id);
        Task<DonationDto> CreateDonationAsync(DonationCreateDto donationDto);
        Task<DonationDto> UpdateDonationAsync(int id, DonationCreateDto donationDto);
        Task<bool> DeleteDonationAsync(int id);
    }
}
