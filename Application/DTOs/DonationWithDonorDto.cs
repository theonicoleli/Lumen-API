using Domain.Entities.Enums;

namespace Application.DTOs
{
    public class DonationWithDonorDto
    {
        public int DonationId { get; set; }
        public string DonationMethod { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public decimal DonationAmount { get; set; }
        public DonationStatusEnum Status { get; set; }
        public bool DonationIsAnonymous { get; set; }
        public string DonationDonorMessage { get; set; } = string.Empty;
        public int DonorUserId { get; set; } 
        public string? DonorName { get; set; }
        public string? DonorDocument { get; set; }
        public string? DonorImageUrl { get; set; }
    }
}