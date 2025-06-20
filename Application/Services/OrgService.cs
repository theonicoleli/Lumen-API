using Application.DTOs;
using Application.Interfaces;
using Infra.Interfaces;

namespace Application.Services
{
    public class OrgService : IOrgService
    {
        private readonly IOrgRepository _orgRepository;
        private readonly ILocalFileStorageService _localFileStorageService;

        public OrgService(IOrgRepository orgRepository, ILocalFileStorageService localFileStorageService)
        {
            _orgRepository = orgRepository;
            _localFileStorageService = localFileStorageService;
        }

        private OrgProfileDto MapToOrgProfileDto(Domain.Entities.Org orgProfileEntity)
        {
            if (orgProfileEntity == null) return null!;

            return new OrgProfileDto
            {
                UserId = orgProfileEntity.UserId,
                OrgName = orgProfileEntity.OrgName,
                Phone = orgProfileEntity.Phone,
                Document = orgProfileEntity.Document,
                Address = orgProfileEntity.Address,
                Description = orgProfileEntity.Description,
                AdminName = orgProfileEntity.AdminName,
                ImageUrl = _localFileStorageService.GetFileUrl(orgProfileEntity.ImageUrl),
                OrgWebsiteUrl = orgProfileEntity.OrgWebsiteUrl,
                OrgFoundationDate = orgProfileEntity.OrgFoundationDate,
                AdminPhone = orgProfileEntity.AdminPhone
            };
        }

        public async Task<IEnumerable<OrgProfileDto>> GetAllOrgsAsync()
        {
            var orgProfileEntities = await _orgRepository.GetAllAsync();
            return orgProfileEntities.Select(MapToOrgProfileDto).ToList();
        }

        public async Task<OrgProfileDto?> GetOrgByUserIdAsync(int userId)
        {
            var orgProfileEntity = await _orgRepository.GetByIdAsync(userId);
            return MapToOrgProfileDto(orgProfileEntity);
        }

    }
}