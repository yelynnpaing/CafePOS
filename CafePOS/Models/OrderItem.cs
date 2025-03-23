namespace CafePOS.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }   
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }               
    }
}