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

        /// <summary>
        /// Retorna todos os perfis de doadores cadastrados.
        /// </summary>
        /// <remarks>Requer permissão de administrador.</remarks>
        /// <returns>Lista de perfis de doadores.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="403">Acesso negado para usuários não administradores.</response>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<IEnumerable<DonorProfileDto>>> GetAllDonorProfiles()
        {
            var donorProfiles = await _donorService.GetAllDonorsAsync();
            return Ok(donorProfiles);
        }

        /// <summary>
        /// Retorna o perfil de doador associado a um usuário específico.
        /// </summary>
        /// <param name="userId">ID do usuário a ser consultado.</param>
        /// <returns>Perfil de doador vinculado ao usuário.</returns>
        /// <response code="200">Perfil encontrado com sucesso.</response>
        /// <response code="403">Usuário autenticado não tem permissão para acessar o perfil de outro usuário.</response>
        /// <response code="404">Usuário ou perfil de doador não encontrado.</response>
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