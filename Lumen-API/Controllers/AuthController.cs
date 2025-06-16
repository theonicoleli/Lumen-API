using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Lumen_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Realiza a autenticação de um usuário e retorna um token JWT.
        /// </summary>
        /// <param name="loginRequest">Credenciais do usuário (e-mail e senha).</param>
        /// <returns>Token JWT se as credenciais forem válidas.</returns>
        /// <response code="200">Login bem-sucedido, token retornado.</response>
        /// <response code="400">Dados inválidos no corpo da requisição.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);

            if (!result.Success || result.User == null)
                return Unauthorized("Credenciais inválidas.");

            return Ok(new { token = result.Token });
        }
    }
}
