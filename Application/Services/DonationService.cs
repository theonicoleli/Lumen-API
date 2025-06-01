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

        private DonationDto MapToDto(Donation donation)
        {
            return new DonationDto
            {
                DonationId = donation.DonationId,
                DonationMethod = donation.DonationMethod,
                DonationDate = donation.DonationDate,
                DonationAmount = donation.DonationAmount,
                Status = donation.Status,
                DonationIsAnonymous = donation.DonationIsAnonymous,
                DonationDonorMessage = donation.DonationDonorMessage,
                DonorId = donation.DonorId,
                OrgId = donation.OrgId,
            };
        }
        private DonationWithDonorDto MapToDonationWithDonorDto(Donation d)
        {
            if (d.Donor == null)
                return new DonationWithDonorDto
                {
                    DonationId = d.DonationId,
                    DonationMethod = d.DonationMethod,
                    DonationDate = d.DonationDate,
                    DonationAmount = d.DonationAmount,
                    Status = d.Status,
                    DonationIsAnonymous = d.DonationIsAnonymous,
                    DonationDonorMessage = d.DonationDonorMessage,
                    DonorUserId = d.DonorId,
                    DonorName = null,
                    DonorDocument = null,
                    DonorImageUrl = null
                };

            return new DonationWithDonorDto
            {
                DonationId = d.DonationId,
                DonationMethod = d.DonationMethod,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                Status = d.Status,
                DonationIsAnonymous = d.DonationIsAnonymous,
                DonationDonorMessage = d.DonationDonorMessage,
                DonorUserId = d.Donor.UserId,
                DonorName = d.Donor.Name,
                DonorDocument = d.Donor.Document,
                DonorImageUrl = d.Donor.ImageUrl
            };
        }


        public async Task<IEnumerable<DonationDto>> GetAllDonationsAsync()
        {
            var donations = await _donationRepository.GetAllAsync();
            return donations.Select(MapToDto).ToList();
        }

        public async Task<DonationDto?> GetDonationByIdAsync(int id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            return donation == null ? null : MapToDto(donation);
        }

        public async Task<DonationDto> CreateDonationAsync(DonationCreateDto dto)
        {
            var donorProfile = await _donorRepository.GetByIdAsync(dto.DonorId);
            if (donorProfile == null)
                throw new KeyNotFoundException($"Perfil de Doador com UserID {dto.DonorId} não encontrado.");

            var orgProfile = await _orgRepository.GetByIdAsync(dto.OrgId);
            if (orgProfile == null)
                throw new KeyNotFoundException($"Perfil de Organização com UserID {dto.OrgId} não encontrado.");

            var donation = new Donation
            {
                DonationMethod = dto.DonationMethod,
                DonationDate = dto.DonationDate,
                DonationAmount = dto.DonationAmount,
                Status = dto.Status,
                DonationIsAnonymous = dto.DonationIsAnonymous,
                DonationDonorMessage = dto.DonationDonorMessage ?? string.Empty,
                DonorId = dto.DonorId,
                OrgId = dto.OrgId
            };

            await _donationRepository.AddAsync(donation);
            await _donationRepository.SaveChangesAsync();

            donation.Donor = donorProfile;
            donation.Org = orgProfile;
            return MapToDto(donation);
        }

        public async Task<DonationDto?> UpdateDonationAsync(int id, DonationCreateDto dto)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null) return null;

            if (dto.DonorId != donation.DonorId)
            {
                var donorProfile = await _donorRepository.GetByIdAsync(dto.DonorId);
                if (donorProfile == null)
                    throw new KeyNotFoundException($"Novo Perfil de Doador com UserID {dto.DonorId} não encontrado.");
                donation.DonorId = dto.DonorId;
                donation.Donor = donorProfile;
            }

            if (dto.OrgId != donation.OrgId)
            {
                var orgProfile = await _orgRepository.GetByIdAsync(dto.OrgId);
                if (orgProfile == null)
                    throw new KeyNotFoundException($"Novo Perfil de Organização com UserID {dto.OrgId} não encontrado.");
                donation.OrgId = dto.OrgId;
                donation.Org = orgProfile;
            }

            donation.DonationMethod = dto.DonationMethod;
            donation.DonationDate = dto.DonationDate;
            donation.DonationAmount = dto.DonationAmount;
            donation.Status = dto.Status;
            donation.DonationIsAnonymous = dto.DonationIsAnonymous;
            donation.DonationDonorMessage = dto.DonationDonorMessage ?? string.Empty;

            await _donationRepository.UpdateAsync(donation);
            await _donationRepository.SaveChangesAsync();

            var updatedDonation = await _donationRepository.GetByIdAsync(id);
            return updatedDonation == null ? null : MapToDto(updatedDonation);
        }

        public async Task<IEnumerable<DonationWithDonorDto>> GetDonationsByDonorAsync(int donorUserId)
        {
            var donations = await _donationRepository.GetByDonorIdAsync(donorUserId);
            if (donations == null || !donations.Any())
                return Enumerable.Empty<DonationWithDonorDto>();

            return donations.Select(MapToDonationWithDonorDto).ToList();
        }

        public async Task<bool> DeleteDonationAsync(int id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null)
                return false;

            await _donationRepository.DeleteAsync(id);
            await _donationRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsByOrgAsync(int orgUserId)
        {
            var donations = await _donationRepository.GetByOrgIdAsync(orgUserId);
            if (donations == null || !donations.Any())
            {
                return Enumerable.Empty<DonationDto>();
            }
            return donations.Select(MapToDto).ToList();
        }
    }
}