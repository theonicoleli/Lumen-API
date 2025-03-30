using Application.DTOs;
using Application.Interfaces;
using Infra.Interfaces;

namespace Application.Services
{
    public class OrgService : IOrgService
    {
        private readonly IOrgRepository _orgRepository;

        public OrgService(IOrgRepository orgRepository)
        {
            _orgRepository = orgRepository;
        }

        public async Task<IEnumerable<OrgDto>> GetAllOrgsAsync()
        {
            var orgs = await _orgRepository.GetAllAsync();
            return orgs.Select(o => new OrgDto
            {
                OrgId = o.OrgId,
                OrgDescription = o.OrgDescription,
                OrgWebsiteUrl = o.OrgWebsiteUrl,
                OrgLocation = o.OrgLocation,
                OrgFoundationDate = o.OrgFoundationDate
            });
        }

        public async Task<OrgDto> GetOrgByIdAsync(int id)
        {
            var org = await _orgRepository.GetByIdAsync(id);
            if (org == null) return null;
            return new OrgDto
            {
                OrgId = org.OrgId,
                OrgDescription = org.OrgDescription,
                OrgWebsiteUrl = org.OrgWebsiteUrl,
                OrgLocation = org.OrgLocation,
                OrgFoundationDate = org.OrgFoundationDate
            };
        }

        public async Task<OrgDto> CreateOrgAsync(OrgCreateDto createDto)
        {
            var org = new Domain.Entities.Org
            {
                OrgDescription = createDto.OrgDescription,
                OrgWebsiteUrl = createDto.OrgWebsiteUrl,
                OrgLocation = createDto.OrgLocation,
                OrgFoundationDate = createDto.OrgFoundationDate
            };

            await _orgRepository.AddAsync(org);
            await _orgRepository.SaveChangesAsync();

            return new OrgDto
            {
                OrgId = org.OrgId,
                OrgDescription = org.OrgDescription,
                OrgWebsiteUrl = org.OrgWebsiteUrl,
                OrgLocation = org.OrgLocation,
                OrgFoundationDate = org.OrgFoundationDate
            };
        }

        public async Task<OrgDto> UpdateOrgAsync(int id, OrgCreateDto orgDto)
        {
            var org = await _orgRepository.GetByIdAsync(id);
            if (org == null) return null;

            org.OrgDescription = orgDto.OrgDescription;
            org.OrgWebsiteUrl = orgDto.OrgWebsiteUrl;
            org.OrgLocation = orgDto.OrgLocation;
            org.OrgFoundationDate = orgDto.OrgFoundationDate;

            await _orgRepository.UpdateAsync(org);
            await _orgRepository.SaveChangesAsync();

            return new OrgDto
            {
                OrgId = org.OrgId,
                OrgDescription = org.OrgDescription,
                OrgWebsiteUrl = org.OrgWebsiteUrl,
                OrgLocation = org.OrgLocation,
                OrgFoundationDate = org.OrgFoundationDate
            };
        }

        public async Task<bool> DeleteOrgAsync(int id)
        {
            await _orgRepository.DeleteAsync(id);
            await _orgRepository.SaveChangesAsync();
            return true;
        }
    }
}
