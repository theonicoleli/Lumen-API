using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;

namespace Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;

        public DonationService(IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
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

        public async Task<DonationDto> CreateDonationAsync(DonationCreateDto donationDto)
        {
            var donation = new Donation
            {
                DonationMethod = donationDto.DonationMethod,
                DonationDate = donationDto.DonationDate,
                DonationAmount = donationDto.DonationAmount,
                DonationStatus = donationDto.DonationStatus,
                DonationIsAnonymous = donationDto.DonationIsAnonymous,
                DonationDonorMessage = donationDto.DonationDonorMessage,
                DonorId = donationDto.DonorId,
                OrgId = donationDto.OrgId
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

        public async Task<bool> DeleteDonationAsync(int id)
        {
            await _donationRepository.DeleteAsync(id);
            await _donationRepository.SaveChangesAsync();
            return true;
        }
    }
}
