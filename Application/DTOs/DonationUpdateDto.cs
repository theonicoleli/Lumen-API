using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class DonationUpdateDto
    {
        [StringLength(100, ErrorMessage = "O método da doação não pode exceder 100 caracteres.")]
        public string? DonationMethod { get; set; }

        public DateTime? DonationDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da doação, se fornecido, deve ser maior que zero.")]
        public decimal? DonationAmount { get; set; }

        public DonationStatusEnum? Status { get; set; }

        [StringLength(500, ErrorMessage = "A mensagem do doador não pode exceder 500 caracteres.")]
        public string? DonationDonorMessage { get; set; }

        public bool? DonationIsAnonymous { get; set; }

        public int? DonorId { get; set; }

        public int? OrgId { get; set; }
    }
}