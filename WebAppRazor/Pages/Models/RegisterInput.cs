using System.ComponentModel.DataAnnotations;

namespace WebAppRazor.Pages.Models
{
    public class RegisterInput
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
