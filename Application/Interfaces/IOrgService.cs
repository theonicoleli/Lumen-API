using Application.DTOs;

namespace Application.Interfaces
{
    public interface IOrgService
    {
        Task<IEnumerable<OrgDto>> GetAllOrgsAsync();
        Task<OrgDto> GetOrgByIdAsync(int id);
        Task<OrgDto> CreateOrgAsync(OrgCreateDto orgDto);
        Task<OrgDto> UpdateOrgAsync(int id, OrgCreateDto orgDto);
        Task<bool> DeleteOrgAsync(int id);
    }
}
