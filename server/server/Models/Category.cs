namespace server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<GiftCategory> GiftCategories { get; set; }
    }
}
