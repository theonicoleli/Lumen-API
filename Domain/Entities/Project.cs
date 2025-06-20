namespace Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image_Url { get; set; } = string.Empty;
        public int OrgId { get; set; }
        public virtual Org Org { get; set; } = null!;
    }
}
