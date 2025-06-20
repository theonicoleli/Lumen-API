using BCrypt.Net;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infra.Interfaces;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IOrgRepository _orgRepository;
        private readonly ILocalFileStorageService _localFileStorageService;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            IDonorRepository donorRepository,
            IOrgRepository orgRepository,
            ILocalFileStorageService localFileStorageService,
            IConfiguration configuration
            )
        {
            _userRepository = userRepository;
            _donorRepository = donorRepository;
            _orgRepository = orgRepository;
            _localFileStorageService = localFileStorageService;
            _configuration = configuration;
        }

        private UserDto MapUserToDto(User user)
        {
            if (user == null) return null!;

            return new UserDto
            {
                Id = user.Id,
                UserEmail = user.UserEmail,
                Role = user.Role,
                UserDateCreated = user.UserDateCreated,
                DonorProfile = user.DonorProfile == null ? null : new DonorProfileDto
                {
                    UserId = user.DonorProfile.UserId,
                    Name = user.DonorProfile.Name,
                    Document = user.DonorProfile.Document,
                    Phone = user.DonorProfile.Phone,
                    BirthDate = user.DonorProfile.BirthDate,
                    ImageUrl = _localFileStorageService.GetFileUrl(user.DonorProfile.ImageUrl)
                },
                OrgProfile = user.OrgProfile == null ? null : new OrgProfileDto
                {
                    UserId = user.OrgProfile.UserId,
                    OrgName = user.OrgProfile.OrgName,
                    Phone = user.OrgProfile.Phone,
                    Document = user.OrgProfile.Document,
                    Address = user.OrgProfile.Address,
                    Description = user.OrgProfile.Description,
                    AdminName = user.OrgProfile.AdminName,
                    ImageUrl = _localFileStorageService.GetFileUrl(user.OrgProfile.ImageUrl),
                    OrgWebsiteUrl = user.OrgProfile.OrgWebsiteUrl,
                    OrgFoundationDate = user.OrgProfile.OrgFoundationDate,
                    AdminPhone = user.OrgProfile.AdminPhone
                }
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapUserToDto).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return MapUserToDto(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return MapUserToDto(user);
        }

        public async Task<UserDto> CreateAdminUserAsync(UserCreateDto dto)
        {
            if (dto.Role != UserRole.Admin)
            {
                throw new ArgumentException("Este método destina-se apenas à criação de usuários administradores.", nameof(dto.Role));
            }

            var existingUser = await _userRepository.GetByEmailAsync(dto.UserEmail);
            if (existingUser != null)
            {
                throw new InvalidOperationException("O e-mail fornecido já está em uso.");
            }

            var user = new User
            {
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                Role = UserRole.Admin,
                UserDateCreated = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public async Task<UserDto> CreateUserWithDonorProfileAsync(UserDonorCreateDto dto, IFormFile? imageFile)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.UserEmail);
            if (existingUser != null)
            {
                throw new InvalidOperationException("O e-mail fornecido já está em uso.");
            }

            string donorImageRelativePath = string.Empty;
            if (imageFile != null && imageFile.Length > 0)
            {
                donorImageRelativePath = await _localFileStorageService.SaveFileAsync(imageFile, "donors");
            }

            var user = new User
            {
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                Role = UserRole.Donor,
                UserDateCreated = DateTime.UtcNow
            };

            user.DonorProfile = new Donor
            {
                User = user,
                Name = dto.Name,
                Document = dto.Document,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                ImageUrl = donorImageRelativePath
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public async Task<UserDto> CreateUserWithOrgProfileAsync(UserOrgCreateDto dto, IFormFile? imageFile)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.UserEmail);
            if (existingUser != null)
            {
                throw new InvalidOperationException("O e-mail fornecido já está em uso.");
            }

            string orgImageRelativePath = string.Empty;
            if (imageFile != null && imageFile.Length > 0)
            {
                orgImageRelativePath = await _localFileStorageService.SaveFileAsync(imageFile, "orgs");
            }

            var user = new User
            {
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                Role = UserRole.Org,
                UserDateCreated = DateTime.UtcNow
            };

            user.OrgProfile = new Domain.Entities.Org
            {
                User = user,
                OrgName = dto.OrgName,
                Phone = dto.OrgPhone,
                Document = dto.OrgDocument,
                Address = dto.OrgAddress,
                Description = dto.OrgDescription,
                AdminName = dto.OrgAdminName,
                ImageUrl = orgImageRelativePath,
                OrgWebsiteUrl = dto.OrgWebsiteUrl ?? string.Empty,
                OrgFoundationDate = dto.OrgFoundationDate,
                AdminPhone = dto.OrgAdminPhone ?? string.Empty
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public async Task<UserDto?> UpdateUserCoreAsync(int userId, UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.UserEmail) && user.UserEmail != dto.UserEmail)
            {
                var existingByEmail = await _userRepository.GetByEmailAsync(dto.UserEmail);
                if (existingByEmail != null && existingByEmail.Id != userId)
                    throw new InvalidOperationException("O novo e-mail fornecido já está em uso por outro usuário.");

                user.UserEmail = dto.UserEmail;
            }

            if (!string.IsNullOrWhiteSpace(dto.UserPassword))
                user.UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword);

            if (dto.Role.HasValue && user.Role != dto.Role.Value)
                user.Role = dto.Role.Value;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return MapUserToDto(user);
        }

        public async Task<OrgProfileDto?> UpdateOrgProfileAsync(int userId, OrgProfileUpdateDto dto, IFormFile? imageFile)
        {
            var orgProfile = await _orgRepository.GetByIdAsync(userId);
            if (orgProfile == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.Role != UserRole.Org) return null;

                orgProfile = new Domain.Entities.Org { UserId = userId, User = user };
                orgProfile.OrgName = dto.OrgName ?? "Nome Padrão";

                string newImageRelativePathOnCreate = string.Empty;
                if (imageFile != null && imageFile.Length > 0)
                    newImageRelativePathOnCreate = await _localFileStorageService.SaveFileAsync(imageFile, "orgs");

                orgProfile.ImageUrl = newImageRelativePathOnCreate;
                await _orgRepository.AddAsync(orgProfile);
            }
            else
            {
                string? oldImagePath = orgProfile.ImageUrl;
                string? newImagePathToSet = orgProfile.ImageUrl;

                if (imageFile != null && imageFile.Length > 0)
                    newImagePathToSet = await _localFileStorageService.SaveFileAsync(imageFile, "orgs");

                else if (dto.ClearImage)
                    newImagePathToSet = string.Empty;

                orgProfile.ImageUrl = newImagePathToSet;

                if (dto.OrgName != null) orgProfile.OrgName = dto.OrgName;
                if (dto.Phone != null) orgProfile.Phone = dto.Phone;
                if (dto.Document != null) orgProfile.Document = dto.Document;
                if (dto.Address != null) orgProfile.Address = dto.Address;
                if (dto.Description != null) orgProfile.Description = dto.Description;
                if (dto.AdminName != null) orgProfile.AdminName = dto.AdminName;
                if (dto.OrgWebsiteUrl != null) orgProfile.OrgWebsiteUrl = dto.OrgWebsiteUrl;
                if (dto.OrgFoundationDate.HasValue) orgProfile.OrgFoundationDate = dto.OrgFoundationDate.Value;
                if (dto.AdminPhone != null) orgProfile.AdminPhone = dto.AdminPhone;

                await _orgRepository.UpdateAsync(orgProfile);

                if (!string.IsNullOrEmpty(oldImagePath) && oldImagePath != orgProfile.ImageUrl)
                {
                    if ((imageFile != null && imageFile.Length > 0) || dto.ClearImage)
                        _localFileStorageService.DeleteFile(oldImagePath);
                }
            }

            await _orgRepository.SaveChangesAsync();
            var updatedOrgForDto = await _orgRepository.GetByIdAsync(userId);
            return MapUserToDto(updatedOrgForDto!.User)?.OrgProfile;
        }


        public async Task<DonorProfileDto?> UpdateDonorProfileAsync(int userId, DonorProfileUpdateDto dto, IFormFile? imageFile)
        {
            var donorProfile = await _donorRepository.GetByIdAsync(userId);
            if (donorProfile == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.Role != UserRole.Donor) return null;

                donorProfile = new Donor { UserId = userId, User = user };
                donorProfile.Name = dto.Name ?? "Nome Padrão";
                donorProfile.Document = dto.Document ?? "";
                donorProfile.Phone = dto.Phone ?? "";
                donorProfile.BirthDate = dto.BirthDate ?? DateTime.MinValue;

                string newImageRelativePathOnCreate = string.Empty;
                if (imageFile != null && imageFile.Length > 0)
                {
                    newImageRelativePathOnCreate = await _localFileStorageService.SaveFileAsync(imageFile, "donors");
                }
                donorProfile.ImageUrl = newImageRelativePathOnCreate;
                await _donorRepository.AddAsync(donorProfile);
            }
            else
            {
                string? oldImagePath = donorProfile.ImageUrl;
                string? newImagePathToSet = donorProfile.ImageUrl;

                if (imageFile != null && imageFile.Length > 0)
                {
                    newImagePathToSet = await _localFileStorageService.SaveFileAsync(imageFile, "donors");
                }
                else if (dto.ClearImage)
                {
                    newImagePathToSet = string.Empty;
                }
                donorProfile.ImageUrl = newImagePathToSet;

                if (dto.Name != null) donorProfile.Name = dto.Name;
                if (dto.Document != null) donorProfile.Document = dto.Document;
                if (dto.Phone != null) donorProfile.Phone = dto.Phone;
                if (dto.BirthDate.HasValue) donorProfile.BirthDate = dto.BirthDate.Value;

                await _donorRepository.UpdateAsync(donorProfile);

                if (!string.IsNullOrEmpty(oldImagePath) && oldImagePath != donorProfile.ImageUrl)
                {
                    if ((imageFile != null && imageFile.Length > 0) || dto.ClearImage)
                        _localFileStorageService.DeleteFile(oldImagePath);
                }
            }

            await _donorRepository.SaveChangesAsync();
            var updatedDonorForDto = await _donorRepository.GetByIdAsync(userId);
            return MapUserToDto(updatedDonorForDto!.User)?.DonorProfile;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            if (user.DonorProfile != null && !string.IsNullOrEmpty(user.DonorProfile.ImageUrl))
            {
                _localFileStorageService.DeleteFile(user.DonorProfile.ImageUrl);
            }
            if (user.OrgProfile != null && !string.IsNullOrEmpty(user.OrgProfile.ImageUrl))
            {
                _localFileStorageService.DeleteFile(user.OrgProfile.ImageUrl);
            }

            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        private string GenerateJwtToken(UserDto user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("As configurações JWT (SecretKey, Issuer, Audience) não foram encontradas ou são inválidas no appsettings.json.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpiresInHours"] ?? "1"));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool Success, string Token, UserDto? User)> ValidateUserAsync(string email, string password)
        {
            var userEntity = await _userRepository.GetByEmailAsync(email);
            if (userEntity == null)
                return (false, string.Empty, null);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, userEntity.UserPassword);
            if (!isPasswordValid)
                return (false, string.Empty, null);

            var userDto = MapUserToDto(userEntity);
            var tokenString = GenerateJwtToken(userDto);

            return (true, tokenString, userDto);
        }
    }
}
