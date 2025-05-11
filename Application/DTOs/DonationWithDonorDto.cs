namespace Application.DTOs
{
    public class DonationWithDonorDto
    {
        public int DonationId { get; set; }
        public string DonationMethod { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public decimal DonationAmount { get; set; }
        public string DonationStatus { get; set; } = string.Empty;
        public bool DonationIsAnonymous { get; set; }
        public string DonationDonorMessage { get; set; } = string.Empty;
        public int DonorId { get; set; }
        public string DonorDocument { get; set; } = string.Empty;
        public string DonorLocation { get; set; } = string.Empty;
    }
}
