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
    [Route("api/orgs")]
    public class OrgsController : ControllerBase
    {
        private readonly IOrgService _orgService;
        private readonly IUserService _userService;

        public OrgsController(IOrgService orgService, IUserService userService)
        {
            _orgService = orgService;
            _userService = userService;
        }

        /// <summary>
        /// Retorna todos os perfis de organizações cadastradas.
        /// </summary>
        /// <remarks>Este endpoint é público e não exige autenticação.</remarks>
        /// <returns>Lista de perfis de organizações.</returns>
        /// <response code="200">Lista de organizações retornada com sucesso.</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrgProfileDto>>> GetAllOrgProfiles()
        {
            var orgProfiles = await _orgService.GetAllOrgsAsync();
            return Ok(orgProfiles);
        }

        /// <summary>
        /// Retorna o perfil de organização associado a um usuário específico.
        /// </summary>
        /// <param name="userId">ID do usuário a ser consultado.</param>
        /// <returns>Perfil da organização vinculada ao usuário.</returns>
        /// <response code="200">Perfil encontrado com sucesso.</response>
        /// <response code="404">Usuário não encontrado ou não possui perfil de organização.</response>
        [HttpGet("{userId}")]
        public async Task<ActionResult<OrgProfileDto>> GetOrgProfileByUserId(int userId)
        {
            // var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            //
            // if (string.IsNullOrEmpty(currentUserIdString) ||
            //     (currentUserIdString != userId.ToString() && currentUserRole != UserRole.Admin.ToString()))
            // {
            //     return Forbid();
            // }

            var orgProfile = await _orgService.GetOrgByUserIdAsync(userId);

            if (orgProfile == null)
            {
                var userCheck = await _userService.GetUserByIdAsync(userId);
                if (userCheck == null)
                    return NotFound($"Usuário com ID {userId} não encontrado.");

                if (userCheck.Role != UserRole.Org || userCheck.OrgProfile == null)
                    return NotFound($"Usuário com ID {userId} não possui um perfil de organização ativo.");

                return NotFound($"Perfil de organização para o usuário com ID {userId} não encontrado.");
            }
            return Ok(orgProfile);
        }

    }
}