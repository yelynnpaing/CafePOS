using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafePOS.Models
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; } = "https://via.placeholder.com/150";
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; } 
        public  DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt { get; set; }
        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}