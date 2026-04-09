using DBO;
using DBO.IServices;
using DBO.Models;
using DBO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.Pages.Models;

namespace WebAppRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService userService;

        public IndexModel(IUserService _service)
        {
            this.userService = _service;
        }

        // This binds the Login form data
        [BindProperty]
        public LoginInput LoginData { get; set; }

        // This binds the Register form data
        [BindProperty]
        public RegisterInput RegisterData { get; set; }

        [TempData]
        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet() { }

        // HANDLER 1: Handles the Login Form
        public async Task<IActionResult> OnPostLoginAsync()
        {
            var result = await userService.GetUserByEmailAsync(LoginData.Email);

            // Check if user exists and password matches
            if (result.IsSuccess && result.Data.Password == LoginData.Password)
            {
                // Successfully logged in! 
                if (result.Data.Role.ToLower() == "admin")
                    return RedirectToPage("/AdminDashboard");
                else
                    return RedirectToPage("/Dashboard");
            }

            ErrorMessage = result.Error ?? "Invalid email or password.";
            return Page();
        }

        // HANDLER 2: Handles the Register Form
        public async Task<IActionResult> OnPostRegisterAsync()
        {
            // Check if user exists
            var check = await userService.IsEmailExistAsync(RegisterData.Email);
            if (check.IsSuccess)
            {
                ErrorMessage = check.Error;
                return Page();
            }

            var newUser = new User
            {
                Email = RegisterData.Email,
                Password = RegisterData.Password,
                Role = "User", // Default role
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var result = await userService.AddUserAsync(newUser);
            if (result.IsSuccess)
            {
                Message = "Registration successful! You can now login.";
                return RedirectToPage(); // Refresh page to show message
            }

            ErrorMessage = "Something went wrong.";
            return Page();
        }
    }
}
