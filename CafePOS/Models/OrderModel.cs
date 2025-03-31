using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafePOS.Models
{
    public class OrderModel
    {
        public OrderModel()
        {
            OrderItems = new List<OrderItem>();
        }

        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }  
        public string FullName { get; set; }
        public string OrderType { get; set; }        
        [Required]
        public Guid CafeTableId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }        
        public string PaymentStatus { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ValidateNever]
        [ForeignKey("CafeTableId")]
        public CafeTable CafeTable { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}