using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do usuário é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string UserPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "O papel (Role) do usuário é obrigatório.")]
        public UserRole Role { get; set; } // Ex: UserRole.admin
    }
}