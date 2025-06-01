using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<UserDto> CreateAdminUserAsync(UserCreateDto dto);
        Task<UserDto> CreateUserWithDonorProfileAsync(UserDonorCreateDto dto, IFormFile? imageFile);
        Task<UserDto> CreateUserWithOrgProfileAsync(UserOrgCreateDto dto, IFormFile? imageFile);

        Task<UserDto?> UpdateUserCoreAsync(int userId, UserUpdateDto dto);
        Task<DonorProfileDto?> UpdateDonorProfileAsync(int userId, DonorProfileUpdateDto dto, IFormFile? imageFile);
        Task<OrgProfileDto?> UpdateOrgProfileAsync(int userId, OrgProfileUpdateDto dto, IFormFile? imageFile);

        Task<bool> DeleteUserAsync(int id); 
        Task<(bool Success, string Token, UserDto? User)> ValidateUserAsync(string email, string password);
    }
}