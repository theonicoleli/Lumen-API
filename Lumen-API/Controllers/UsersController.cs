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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString) ||
                (currentUserIdString != id.ToString() && !User.IsInRole(UserRole.Admin.ToString())))
            {
                return Forbid();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }
            return Ok(user);
        }

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

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Donor)},{nameof(UserRole.Org)}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("{userId}/core")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUserCore(int userId, [FromBody] UserUpdateDto dto)
        {
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString) ||
                (currentUserIdString != userId.ToString() && !User.IsInRole(UserRole.Admin.ToString())))
            {
                return Forbid();
            }

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

        [HttpPut("{userId}/profile/donor")]
        [Authorize]
        public async Task<ActionResult<DonorProfileDto>> UpdateDonorProfile(int userId, [FromForm] DonorProfileUpdateDto dto, IFormFile? imageFile)
        {
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString) ||
                (currentUserIdString != userId.ToString() && !User.IsInRole(UserRole.Admin.ToString())))
            {
                return Forbid();
            }

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

        [HttpPut("{userId}/profile/org")]
        [Authorize]
        public async Task<ActionResult<OrgProfileDto>> UpdateOrgProfile(int userId, [FromForm] OrgProfileUpdateDto dto, IFormFile? imageFile)
        {
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString) ||
                (currentUserIdString != userId.ToString() && !User.IsInRole(UserRole.Admin.ToString())))
            {
                return Forbid();
            }

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