using Domain.Entities.Enums;

namespace Application.DTOs
{
    public class DonationDto
    {
        public int DonationId { get; set; }
        public string DonationMethod { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public decimal DonationAmount { get; set; }
        public DonationStatusEnum Status { get; set; }
        public bool DonationIsAnonymous { get; set; }
        public string DonationDonorMessage { get; set; } = string.Empty;
        public int DonorId { get; set; }
        public int OrgId { get; set; }
        public string? DonorName { get; set; }
        public string? OrgName { get; set; }
        public string? DonorImageUrl { get; set; }
        public string? OrgImageUrl { get; set; }
    }
}