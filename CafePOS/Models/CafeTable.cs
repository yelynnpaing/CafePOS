using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CafePOS.Models
{
    public class CafeTable
    {
        [Key]
        public Guid CafeTableId { get; set; }
        [Required]
        public int TableNumber { get; set; }
        [ValidateNever]          
        public string? Note { get; set; }
        [ValidateNever]
        public ICollection<Order>? Orders { get; set; }

    }
}
