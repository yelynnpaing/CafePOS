namespace CafePOS.Models
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public int StockQty { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
