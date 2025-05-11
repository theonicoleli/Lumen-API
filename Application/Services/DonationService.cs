using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;

namespace Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IOrgRepository _orgRepository;

        public DonationService(IDonationRepository donationRepository, IDonorRepository donorRepository, IOrgRepository orgRepository)
        {
            _donationRepository = donationRepository;
            _donorRepository = donorRepository;
            _orgRepository = orgRepository;
        }

        public async Task<IEnumerable<DonationDto>> GetAllDonationsAsync()
        {
            var donations = await _donationRepository.GetAllAsync();
            return donations.Select(d => new DonationDto
            {
                DonationId = d.DonationId,
                DonationMethod = d.DonationMethod,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonationStatus = d.DonationStatus,
                DonationIsAnonymous = d.DonationIsAnonymous,
                DonationDonorMessage = d.DonationDonorMessage,
                DonorId = d.DonorId,
                OrgId = d.OrgId
            });
        }

        public async Task<DonationDto> GetDonationByIdAsync(int id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null) return null;
            return new DonationDto
            {
                DonationId = donation.DonationId,
                DonationMethod = donation.DonationMethod,
                DonationDate = donation.DonationDate,
                DonationAmount = donation.DonationAmount,
                DonationStatus = donation.DonationStatus,
                DonationIsAnonymous = donation.DonationIsAnonymous,
                DonationDonorMessage = donation.DonationDonorMessage,
                DonorId = donation.DonorId,
                OrgId = donation.OrgId
            };
        }

        public async Task<DonationDto> CreateDonationAsync(DonationCreateDto dto)
        {
            var donor = await _donorRepository.GetByIdAsync(dto.DonorId);
            if (donor == null)
                throw new KeyNotFoundException($"Donor {dto.DonorId} não encontrado.");

            var org = await _orgRepository.GetByIdAsync(dto.OrgId);
            if (org == null)
                throw new KeyNotFoundException($"Org {dto.OrgId} não encontrada.");

            var donation = new Donation
            {
                DonationMethod = dto.DonationMethod,
                DonationDate = dto.DonationDate,
                DonationAmount = dto.DonationAmount,
                DonationStatus = dto.DonationStatus,
                DonationIsAnonymous = dto.DonationIsAnonymous,
                DonationDonorMessage = dto.DonationDonorMessage,
                DonorId = dto.DonorId,
                OrgId = dto.OrgId
            };

            await _donationRepository.AddAsync(donation);
            await _donationRepository.SaveChangesAsync();

            return new DonationDto
            {
                DonationId = donation.DonationId,
                DonationMethod = donation.DonationMethod,
                DonationDate = donation.DonationDate,
                DonationAmount = donation.DonationAmount,
                DonationStatus = donation.DonationStatus,
                DonationIsAnonymous = donation.DonationIsAnonymous,
                DonationDonorMessage = donation.DonationDonorMessage,
                DonorId = donation.DonorId,
                OrgId = donation.OrgId
            };
        }

        public async Task<DonationDto> UpdateDonationAsync(int id, DonationCreateDto donationDto)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null) return null;

            donation.DonationMethod = donationDto.DonationMethod;
            donation.DonationDate = donationDto.DonationDate;
            donation.DonationAmount = donationDto.DonationAmount;
            donation.DonationStatus = donationDto.DonationStatus;
            donation.DonationIsAnonymous = donationDto.DonationIsAnonymous;
            donation.DonationDonorMessage = donationDto.DonationDonorMessage;
            donation.DonorId = donationDto.DonorId;
            donation.OrgId = donationDto.OrgId;

            await _donationRepository.UpdateAsync(donation);
            await _donationRepository.SaveChangesAsync();

            return new DonationDto
            {
                DonationId = donation.DonationId,
                DonationMethod = donation.DonationMethod,
                DonationDate = donation.DonationDate,
                DonationAmount = donation.DonationAmount,
                DonationStatus = donation.DonationStatus,
                DonationIsAnonymous = donation.DonationIsAnonymous,
                DonationDonorMessage = donation.DonationDonorMessage,
                DonorId = donation.DonorId,
                OrgId = donation.OrgId
            };
        }

        public async Task<IEnumerable<DonationWithDonorDto>> GetDonationsByDonorAsync(int donorId)
        {
            var donations = await _donationRepository.GetByDonorIdAsync(donorId);

            return donations.Select(d => new DonationWithDonorDto
            {
                DonationId = d.DonationId,
                DonationMethod = d.DonationMethod,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonationStatus = d.DonationStatus,
                DonationIsAnonymous = d.DonationIsAnonymous,
                DonationDonorMessage = d.DonationDonorMessage,

                DonorId = d.Donor.DonorId,
                DonorDocument = d.Donor.DonorDocument,
                DonorLocation = d.Donor.DonorLocation
            });
        }

        public async Task<bool> DeleteDonationAsync(int id)
        {
            await _donationRepository.DeleteAsync(id);
            await _donationRepository.SaveChangesAsync();
            return true;
        }
    }
}
