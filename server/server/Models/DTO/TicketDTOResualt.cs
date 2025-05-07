namespace server.Models.DTO
{
    public class TicketDTOResualt
    {
        public bool isWin { get; set; } = false;
        public bool isInBasket { get; set; } = true;
        public int Amount { get; set; }
        public GiftDTOTheen Gift { get; set; }

        public UserDTOResualt? Winner { get; set; }
    }
}
