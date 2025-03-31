using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Identity.Client;

namespace CafePOS.Models
{
    public class Users : IdentityUser
    {
        public Users()
        {
            Orders = new List<Order>();
        }

        public string FullName { get; set; }
        //[ValidateNever]
        //public string UserRole { get; set; }
        [ValidateNever]
        public List<string>? CurrentRoles { get; set; }
        [ValidateNever]
        public List<string>? AvailableRoles { get; set; }
        [ValidateNever]
        public string? NewRole { get; set; }
        [ValidateNever]
        public string UserStatus { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
