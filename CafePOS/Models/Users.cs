using Microsoft.AspNetCore.Identity;

namespace CafePOS.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
