using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;

namespace Application.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;

        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        public async Task<IEnumerable<DonorDto>> GetAllDonorsAsync()
        {
            var donors = await _donorRepository.GetAllAsync();
            return donors.Select(d => new DonorDto
            {
                DonorId = d.DonorId,
                DonorDocument = d.DonorDocument,
                DonorLocation = d.DonorLocation
            });
        }

        public async Task<DonorDto> GetDonorByIdAsync(int id)
        {
            var donor = await _donorRepository.GetByIdAsync(id);
            if (donor == null) return null;
            return new DonorDto
            {
                DonorId = donor.DonorId,
                DonorDocument = donor.DonorDocument,
                DonorLocation = donor.DonorLocation
            };
        }

        public async Task<DonorDto> CreateDonorAsync(DonorCreateDto donorDto)
        {
            var donor = new Donor
            {
                DonorDocument = donorDto.DonorDocument,
                DonorLocation = donorDto.DonorLocation
            };

            await _donorRepository.AddAsync(donor);
            await _donorRepository.SaveChangesAsync();

            return new DonorDto
            {
                DonorId = donor.DonorId,
                DonorDocument = donor.DonorDocument,
                DonorLocation = donor.DonorLocation
            };
        }

        public async Task<DonorDto> UpdateDonorAsync(int id, DonorCreateDto donorDto)
        {
            var donor = await _donorRepository.GetByIdAsync(id);
            if (donor == null) return null;

            donor.DonorDocument = donorDto.DonorDocument;
            donor.DonorLocation = donorDto.DonorLocation;

            await _donorRepository.UpdateAsync(donor);
            await _donorRepository.SaveChangesAsync();

            return new DonorDto
            {
                DonorId = donor.DonorId,
                DonorDocument = donor.DonorDocument,
                DonorLocation = donor.DonorLocation
            };
        }

        public async Task<bool> DeleteDonorAsync(int id)
        {
            await _donorRepository.DeleteAsync(id);
            await _donorRepository.SaveChangesAsync();
            return true;
        }
    }
}
