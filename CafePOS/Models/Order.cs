namespace CafePOS.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }    
        public bool OrderType { get; set; }
        public int TableNumber { get; set; }    
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}