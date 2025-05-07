using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace server.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Details { get; set; }

        [Range(10, 100, ErrorMessage = "The price must be between 10 and 100")]
        public int Price { get; set; } = 10;

        [Range(1, 3, ErrorMessage = "The size must be between 1 and 3")]
        public int Size { get; set; } = 1;
        public string? Image { get; set; }
        public int UserWinnerId { get; set; } = 0;
        public User? Winner { get; set; }
        public List<DonorGift> DonorGifts { get; set; }
        public List<GiftCategory>? GiftCategories { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
