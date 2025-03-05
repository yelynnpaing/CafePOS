using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafePOS.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; } = "https://via.placeholder.com/150";
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public  DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt { get; set; }
    }
}