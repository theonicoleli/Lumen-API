using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserDonorCreateDto
    {
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do usuário é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string UserPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do doador é obrigatório.")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O documento (CPF) do doador é obrigatório.")]
        [StringLength(50)]
        public string Document { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone do doador é obrigatório.")]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento do doador é obrigatória.")]
        public DateTime BirthDate { get; set; }
    }
}