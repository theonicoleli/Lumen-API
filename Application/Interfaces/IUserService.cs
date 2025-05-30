﻿using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(UserCreateOrUpdateDto dto);
        Task<UserDto> CreateUserAndDonorAsync(UserDonorCreateDto dto);
        Task<UserDto> UpdateUserAsync(int id, UserCreateOrUpdateDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> ValidateUserAsync(string email, string password);
    }
}
