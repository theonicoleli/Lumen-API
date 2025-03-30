using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;
using Infra.Repositories;
using Infra.Services;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IFirebaseStorageService _firebaseService;

        public UserService(IUserRepository userRepository, IDonorRepository donorRepository, IFirebaseStorageService firebaseService)
        {
            _userRepository = userRepository;
            _donorRepository = donorRepository;
            _firebaseService = firebaseService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                UserEmail = u.UserEmail,
                UserPassword = u.UserPassword,
                UserStatus = u.UserStatus,
                UserImageUrl = u.UserImageUrl,
                DonorId = u.DonorId,
            });
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl,
                DonorId = user.DonorId
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateOrUpdateDto userDto)
        {

            //string imageUrl = dto.ImageFile != null
            //    ? await _firebaseService.UploadFileAsync(dto.ImageFile)
            //    : null;

            var donor = new Donor
            {
                DonorDocument = string.Empty,
                DonorLocation = string.Empty
            };

            await _donorRepository.AddAsync(donor);
            await _donorRepository.SaveChangesAsync();

            var user = new User
            {
                UserEmail = userDto.UserEmail,
                UserPassword = userDto.UserPassword,
                UserStatus = userDto.UserStatus,
                UserImageUrl = "",
                DonorId = donor.DonorId
            };
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl,
                DonorId = user.DonorId
            };
        }


        public async Task<UserDto> UpdateUserAsync(int id, UserCreateOrUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            //if (dto.ImageFile != null)
            //{
            //    var imageUrl = await _firebaseService.UploadFileAsync(dto.ImageFile);
            //    user.UserImageUrl = imageUrl;
            //}

            user.UserEmail = dto.UserEmail;
            user.UserPassword = dto.UserPassword;
            user.UserStatus = dto.UserStatus;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAndPasswordAsync(email, password);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
            };
        }
    }
}
