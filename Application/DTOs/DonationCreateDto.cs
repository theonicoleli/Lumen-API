using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class DonationCreateDto
    {
        [Required]
        public string DonationMethod { get; set; } = string.Empty;
        [Required]
        public DateTime DonationDate { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal DonationAmount { get; set; }
        [Required]
        public DonationStatusEnum Status { get; set; }
        public bool DonationIsAnonymous { get; set; }
        public string DonationDonorMessage { get; set; } = string.Empty;
        [Required]
        public int DonorId { get; set; }
        [Required]
        public int OrgId { get; set; }
    }
}