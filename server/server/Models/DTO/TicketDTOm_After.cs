namespace server.Models.DTO
{
    public class TicketDTOm_After
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public UserDTOResualt winner { get; set; } = null;
        public int Sales { get; set; }
    }
}
