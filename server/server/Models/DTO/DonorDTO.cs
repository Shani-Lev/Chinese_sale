namespace server.Models.DTO
{
    public class DonorDTO
    {
        public string Name { get; set; }
        public string? Details { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Logo { get; set; }
        public bool? ShowMe { get; set; } = false;
    }
}
