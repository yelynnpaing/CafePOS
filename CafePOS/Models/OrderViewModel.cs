namespace CafePOS.Models
{
    public class OrderViewModel
    {
        public decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> OrderItems {get; set;}
        public IEnumerable<Item> Items {get; set;}
    }
}
