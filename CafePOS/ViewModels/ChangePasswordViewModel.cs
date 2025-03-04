using System.ComponentModel.DataAnnotations;

namespace CafePOS.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "New Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and {1} Characters long.")]
        [Compare("NewConfirmPassword", ErrorMessage = "Password does not match with New confirm password")]
        [Display(Name = "Confirm Password")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "New Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Confirm Password")]
        public string NewConfirmPassword { get; set; }
    }
}
