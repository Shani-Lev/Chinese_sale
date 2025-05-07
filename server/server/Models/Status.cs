namespace server.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? LottoryEnd { get; set; }
        public DateTime UpdateTo { get; set; }
    }
}
