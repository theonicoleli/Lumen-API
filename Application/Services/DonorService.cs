using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;

namespace Application.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        private readonly ILocalFileStorageService _localFileStorageService;

        public DonorService(IDonorRepository donorRepository, ILocalFileStorageService localFileStorageService)
        {
            _donorRepository = donorRepository;
            _localFileStorageService = localFileStorageService;
        }

        private DonorProfileDto MapToDonorProfileDto(Donor donorProfileEntity)
        {
            if (donorProfileEntity == null) return null!;
            return new DonorProfileDto
            {
                UserId = donorProfileEntity.UserId,
                Name = donorProfileEntity.Name,
                Document = donorProfileEntity.Document,
                Phone = donorProfileEntity.Phone,
                BirthDate = donorProfileEntity.BirthDate,
                ImageUrl = _localFileStorageService.GetFileUrl(donorProfileEntity.ImageUrl)
            };
        }

        public async Task<IEnumerable<DonorProfileDto>> GetAllDonorsAsync()
        {
            var donorProfileEntities = await _donorRepository.GetAllAsync();
            return donorProfileEntities.Select(MapToDonorProfileDto).ToList();
        }

        public async Task<DonorProfileDto?> GetDonorByUserIdAsync(int userId)
        {
            var donorProfileEntity = await _donorRepository.GetByUserIdAsync(userId);
            return MapToDonorProfileDto(donorProfileEntity);
        }
    }
}