namespace server.Models
{
    public class GiftCategory
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public int CategoryId { get; set; }

        public Gift Gift { get; set; }
        public Category Category { get; set; }
    }
}
