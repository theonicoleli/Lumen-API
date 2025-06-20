using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities.Enums;
using System;

namespace Lumen_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registra um novo usuário com papel de administrador.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados do usuário administrador.</param>
        /// <returns>O usuário criado, com ID e informações cadastradas.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="400">Erro de validação ou papel inválido.</response>
        /// <response code="409">Usuário já existente.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [AllowAnonymous]
        [HttpPost("register/admin")]
        public async Task<ActionResult<UserDto>> RegisterAdmin([FromBody] UserCreateDto dto)
        {
            if (dto.Role != UserRole.Admin)
            {
                return BadRequest("Este endpoint é apenas para registro de administradores.");
            }
            try
            {
                var createdUser = await _userService.CreateAdminUserAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        /// <summary>
        /// Registra um novo usuário com perfil de doador.
        /// </summary>
        /// <param name="dto">Dados do doador, como nome, e-mail, senha etc.</param>
        /// <param name="imageFile">Imagem de perfil opcional (formato multipart/form-data).</param>
        /// <returns>Dados do usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="400">Erro de validação nos dados.</response>
        /// <response code="409">Conflito: e-mail ou outro campo já cadastrado.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [AllowAnonymous]
        [HttpPost("register/donor")]
        public async Task<ActionResult<UserDto>> RegisterDonor([FromForm] UserDonorCreateDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdUser = await _userService.CreateUserWithDonorProfileAsync(dto, imageFile);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        /// <summary>
        /// Registra um novo usuário com perfil de organização.
        /// </summary>
        /// <param name="dto">Dados da organização, como nome fantasia, CNPJ, descrição etc.</param>
        /// <param name="imageFile">Imagem de perfil opcional da organização.</param>
        /// <returns>Dados da organização criada.</returns>
        [AllowAnonymous]
        [HttpPost("register/org")]
        public async Task<ActionResult<UserDto>> RegisterOrg([FromForm] UserOrgCreateDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdUser = await _userService.CreateUserWithOrgProfileAsync(dto, imageFile);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        /// <summary>
        /// Retorna os dados de um usuário a partir do ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser consultado.</param>
        /// <returns>Objeto com as informações do usuário.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Busca um usuário pelo e-mail (somente administradores).
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <returns>Informações completas do usuário.</returns>
        [HttpGet("byemail/{email}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Usuário com email {email} não encontrado.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Lista todos os usuários cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Donor)},{nameof(UserRole.Org)}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Atualiza informações básicas (core) de um usuário.
        /// </summary>
        /// <param name="userId">ID do usuário a ser atualizado.</param>
        /// <param name="dto">Dados atualizados do usuário.</param>
        /// <returns>Usuário atualizado.</returns>
        [HttpPut("{userId}/core")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUserCore(int userId, [FromBody] UserUpdateDto dto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserCoreAsync(userId, dto);
                if (updatedUser == null)
                {
                    return NotFound($"Usuário com ID {userId} não encontrado para atualização.");
                }
                return Ok(updatedUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        /// <summary>
        /// Atualiza ou cria o perfil de doador para um usuário existente.
        /// </summary>
        /// <param name="userId">ID do usuário.</param>
        /// <param name="dto">Informações do perfil de doador.</param>
        /// <param name="imageFile">Imagem de perfil opcional.</param>
        /// <returns>Perfil de doador atualizado.</returns>
        [HttpPut("{userId}/profile/donor")]
        [Authorize]
        public async Task<ActionResult<DonorProfileDto>> UpdateDonorProfile(int userId, [FromForm] DonorProfileUpdateDto dto, IFormFile? imageFile)
        {
            var userToUpdate = await _userService.GetUserByIdAsync(userId);
            if (userToUpdate == null) return NotFound($"Usuário com ID {userId} não encontrado.");
            if (!User.IsInRole(UserRole.Admin.ToString()) && userToUpdate.Role != UserRole.Donor)
                return BadRequest("Este usuário não é um doador e seu perfil de doador não pode ser atualizado por você.");

            try
            {
                var updatedProfile = await _userService.UpdateDonorProfileAsync(userId, dto, imageFile);
                if (updatedProfile == null)
                    return BadRequest($"Não foi possível atualizar/criar o perfil de doador para o usuário com ID {userId}. Verifique se o usuário existe e tem o papel 'donor'.");

                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        /// <summary>
        /// Atualiza ou cria o perfil de organização para um usuário existente.
        /// </summary>
        /// <param name="userId">ID do usuário.</param>
        /// <param name="dto">Informações do perfil da organização.</param>
        /// <param name="imageFile">Imagem de perfil opcional.</param>
        /// <returns>Perfil de organização atualizado.</returns>
        [HttpPut("{userId}/profile/org")]
        [Authorize]
        public async Task<ActionResult<OrgProfileDto>> UpdateOrgProfile(int userId, [FromForm] OrgProfileUpdateDto dto, IFormFile? imageFile)
        {
            var userToUpdate = await _userService.GetUserByIdAsync(userId);
            if (userToUpdate == null) return NotFound($"Usuário com ID {userId} não encontrado.");
            if (!User.IsInRole(UserRole.Admin.ToString()) && userToUpdate.Role != UserRole.Org)
                return BadRequest("Este usuário não é uma organização e seu perfil de organização não pode ser atualizado por você.");

            try
            {
                var updatedProfile = await _userService.UpdateOrgProfileAsync(userId, dto, imageFile);
                if (updatedProfile == null)
                    return BadRequest($"Não foi possível atualizar/criar o perfil de organização para o usuário com ID {userId}. Verifique se o usuário existe e tem o papel 'org'.");

                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação.");
            }
        }

        /// <summary>
        /// Exclui um usuário do sistema (somente administradores).
        /// </summary>
        /// <param name="id">ID do usuário a ser removido.</param>
        /// <returns>Sem conteúdo se a exclusão for bem-sucedida.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound($"Usuário com ID {id} não encontrado para exclusão.");
            }
            return NoContent();
        }
    }
}