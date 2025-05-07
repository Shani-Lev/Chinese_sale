namespace server.Models.DTO
{
    public class GiftDTOTheen
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Details { get; set; }
        public int Price { get; set; } = 10;
        public int Size { get; set; } = 1;
        public string? Image { get; set; }
    }
}
