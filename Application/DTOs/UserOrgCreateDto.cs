using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserOrgCreateDto
    {
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do usuário é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string UserPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome da organização é obrigatório.")]
        [StringLength(255)]
        public string OrgName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone da organização é obrigatório.")]
        [StringLength(50)]
        public string OrgPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O documento (CNPJ) da organização é obrigatório.")]
        [StringLength(50)]
        public string OrgDocument { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço da organização é obrigatório.")]
        [StringLength(500)]
        public string OrgAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição da organização é obrigatória.")]
        [StringLength(1000)]
        public string OrgDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do administrador da organização é obrigatório.")]
        [StringLength(255)]
        public string OrgAdminName { get; set; } = string.Empty;

        [StringLength(2048)]
        public string? OrgWebsiteUrl { get; set; }

        [Required(ErrorMessage = "A data de fundação da organização é obrigatória.")]
        public DateTime OrgFoundationDate { get; set; }

        [StringLength(50)]
        public string? OrgAdminPhone { get; set; }
    }
}