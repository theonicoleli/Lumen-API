using Application.DTOs;

namespace Application.Interfaces
{
    public interface IOrgService
    {
        Task<OrgProfileDto?> GetOrgByUserIdAsync(int userId);
        Task<IEnumerable<OrgProfileDto>> GetAllOrgsAsync();
    }
}
