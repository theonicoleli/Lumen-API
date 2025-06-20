using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class DonorProfileUpdateDto
    {
        [StringLength(255, ErrorMessage = "O nome não pode exceder 255 caracteres.")]
        public string? Name { get; set; }

        [StringLength(14, ErrorMessage = "O documento deve ter no máximo 14 caracteres.")]
        public string? Document { get; set; }

        [StringLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres.")]
        public string? Phone { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool ClearImage { get; set; } = false;
    }
}