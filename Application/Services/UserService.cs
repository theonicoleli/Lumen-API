using BCrypt.Net;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;
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
                UserStatus = u.UserStatus,
                UserImageUrl = u.UserImageUrl,
                DonorId = u.DonorId,
                UserBirthDate = u.BirthDate,
                UserDateCreated = u.UserDateCreated,
                UserPhone = u.Phone
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
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl,
                DonorId = user.DonorId,
                UserBirthDate = user.BirthDate,
                UserDateCreated = user.UserDateCreated,
                UserPhone = user.Phone
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateOrUpdateDto userDto)
        {
            // Caso exista upload de imagem, pode ser descomentado:
            // string imageUrl = userDto.ImageFile != null ? await _firebaseService.UploadFileAsync(userDto.ImageFile) : "";

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
                UserPassword = BCrypt.Net.BCrypt.HashPassword(userDto.UserPassword),
                UserStatus = userDto.UserStatus,
                UserImageUrl = "",
                DonorId = donor.DonorId,
                BirthDate = userDto.UserBirthDate,
                UserDateCreated = default,
                Phone = userDto.UserPhone
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl,
                DonorId = user.DonorId,
                UserBirthDate = user.BirthDate,
                UserDateCreated = user.UserDateCreated,
                UserPhone = user.Phone
            };
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserCreateOrUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            // if (dto.ImageFile != null)
            // {
            //     var imageUrl = await _firebaseService.UploadFileAsync(dto.ImageFile);
            //     user.UserImageUrl = imageUrl;
            // }

            if (!string.IsNullOrWhiteSpace(dto.UserEmail))
                user.UserEmail = dto.UserEmail;

            if (!string.IsNullOrWhiteSpace(dto.UserPassword))
                user.UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword);

            if (!string.IsNullOrWhiteSpace(dto.UserStatus))
                user.UserStatus = dto.UserStatus;

            if (dto.UserBirthDate != default(DateTime))
                user.BirthDate = dto.UserBirthDate;

            if (!string.IsNullOrWhiteSpace(dto.UserPhone))
                user.Phone = dto.UserPhone;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl,
                DonorId = user.DonorId,
                UserBirthDate = user.BirthDate,
                UserDateCreated = user.UserDateCreated,
                UserPhone = user.Phone
            };
        }
        public async Task<UserDto> CreateUserAndDonorAsync(UserDonorCreateDto dto)
        {
            var donor = new Donor
            {
                DonorDocument = dto.DonorDocument,
                DonorLocation = dto.DonorLocation
            };

            await _donorRepository.AddAsync(donor);
            await _donorRepository.SaveChangesAsync();

            var user = new User
            {
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                UserStatus = dto.UserStatus,
                UserImageUrl = "",
                DonorId = donor.DonorId,
                BirthDate = dto.UserBirthDate,
                UserDateCreated = default,
                Phone = dto.UserPhone
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserStatus = user.UserStatus,
                UserImageUrl = user.UserImageUrl,
                DonorId = user.DonorId,
                UserBirthDate = user.BirthDate,
                UserDateCreated = user.UserDateCreated,
                UserPhone = user.Phone
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
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            var ok = BCrypt.Net.BCrypt.Verify(password, user.UserPassword);
            if (!ok) return null;

            return new UserDto
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail
            };
        }
    }
}
