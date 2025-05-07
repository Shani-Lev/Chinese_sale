namespace server.Models
{
    public class DonorGift
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int GiftId { get; set; }

        public Donor Donor { get; set; }
        public Gift Gift { get; set; }
    }
}
