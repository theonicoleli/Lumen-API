using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserUpdateDto
    {
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string? UserEmail { get; set; }

        [MinLength(6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres.")]
        public string? UserPassword { get; set; }

        public UserRole? Role { get; set; }
    }
}