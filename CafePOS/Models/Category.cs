namespace CafePOS.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CatName { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}