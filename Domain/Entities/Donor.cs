namespace Domain.Entities
{
    public class Donor
    {
        public int DonorId { get; set; }
        public string DonorDocument { get; set; } = string.Empty;
        public string DonorLocation { get; set; } = string.Empty;
        public ICollection<Donation> Donations { get; set; }
    }
}
