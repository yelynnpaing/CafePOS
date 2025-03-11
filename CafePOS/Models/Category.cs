using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CafePOS.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string CatName { get; set; }
        [ValidateNever]
        public ICollection<Item> Items { get; set; }
    }
}