using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Lumen_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/donors")]
    public class DonorsController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly IUserService _userService;

        public DonorsController(IDonorService donorService, IUserService userService)
        {
            _donorService = donorService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<IEnumerable<DonorProfileDto>>> GetAllDonorProfiles()
        {
            var donorProfiles = await _donorService.GetAllDonorsAsync();
            return Ok(donorProfiles);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<DonorProfileDto>> GetDonorProfileByUserId(int userId)
        {
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(currentUserIdString) ||
                (currentUserIdString != userId.ToString() && currentUserRole != UserRole.Admin.ToString()))
            {
                return Forbid();
            }

            var donorProfile = await _donorService.GetDonorByUserIdAsync(userId);

            if (donorProfile == null)
            {
                var userCheck = await _userService.GetUserByIdAsync(userId);
                if (userCheck == null)
                    return NotFound($"Usuário com ID {userId} não encontrado.");

                if (userCheck.Role != UserRole.Donor || userCheck.DonorProfile == null)
                    return NotFound($"Usuário com ID {userId} não possui um perfil de doador ativo.");

                return NotFound($"Perfil de doador para o usuário com ID {userId} não encontrado.");
            }
            return Ok(donorProfile);
        }
    }
}