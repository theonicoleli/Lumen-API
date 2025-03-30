namespace Domain.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminRole { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string AdminContent { get; set; } = string.Empty;
    }
}
