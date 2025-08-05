using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Full Name is required")] 
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required")] 
        [EmailAddress(ErrorMessage = "Invalid Email Address")] 
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")] 
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")] 
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")] 
        [Compare("Password", ErrorMessage = "Passwords do not match")] 
        public string ConfirmPassword { get; set; }
    }
}