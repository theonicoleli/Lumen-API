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
