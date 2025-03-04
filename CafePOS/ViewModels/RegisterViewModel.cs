using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CafePOS.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and {1} character long.")]
        [Compare("ConfirmPassword", ErrorMessage ="Password does not match with confirm password")]
        [Display(Name= "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is require.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Pasword")]
        public string ConfirmPassword { get; set; }

    }
}
