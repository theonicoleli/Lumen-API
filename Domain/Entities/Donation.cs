using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Donation
    {
        public int DonationId { get; set; }
        public string DonationMethod { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public decimal DonationAmount { get; set; }
        public DonationStatusEnum Status { get; set; }
        public string DonationDonorMessage { get; set; } = string.Empty;
        public bool DonationIsAnonymous { get; set; }
        public int DonorId { get; set; }
        public virtual Donor Donor { get; set; } = null!;
        public int OrgId { get; set; }
        public virtual Org Org { get; set; } = null!;
    }
}
