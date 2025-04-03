namespace Domain.Entities
{
    public class Donation
    {
        public int DonationId { get; set; }
        public string DonationMethod { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public decimal DonationAmount { get; set; }
        public string DonationStatus { get; set; } = string.Empty;
        public bool DonationIsAnonymous { get; set; }
        public string DonationDonorMessage { get; set; } = string.Empty;
        public int DonorId { get; set; }
        public Donor Donor { get; set; } = new Donor();
        public int OrgId { get; set; }
        public Org Org { get; set; } = new Org();
    }
}
