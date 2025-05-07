using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Logo { get; set; }
        public bool? ShowMe { get; set; } = false;

        public List<DonorGift> DonorGifts { get; set; }

    }
}
