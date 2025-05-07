namespace server.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GiftId { get; set; }
        public bool isWin { get; set; } = false;
        public bool isInBasket { get; set; } = true;    

        public User User { get; set; }
        public Gift Gift { get; set; }
    }
}
