namespace CafePOS.Models
{
    public class OrderItemViewModel
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}