using DBO.IServices;
using DBO.Models;
using DBO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.Pages.Models;

namespace WebAppRazor.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserService userService;
        public ForgotPasswordModel(IUserService userService)
        {
            this.userService = userService;
        }
        public void OnGet() { }

        [BindProperty]
        public bool IsEmailFound { get; set; } = false;
        [BindProperty]
        public ResetInput RI { get; set; } = new();

        public string ErrorMessage { get; set; }

        // STEP 1: Find the Email
        public async Task<IActionResult> OnPostFindEmailAsync()
        {
            var result = await userService.IsEmailExistAsync(RI.Email);

            if (result.IsSuccess)
            {
                IsEmailFound = true; // Switch to the password fields
                return Page();
            }

            ErrorMessage = "Account not found with this email.";
            return Page();
        }

        // STEP 2: Update the Password
        public async Task<IActionResult> OnPostResetPasswordAsync()
        {
            // Check if passwords match
            if (RI.NewPassword != RI.ConfirmPassword)
            {
                IsEmailFound = true; // Keep the password fields visible
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            var result = await userService.ResetPasswordAsync(RI.NewPassword, RI.Email);

            if (result.IsSuccess)
            {
                TempData["Message"] = "Password updated! Please login.";
                return RedirectToPage("/Index");
            }

            IsEmailFound = true;
            ErrorMessage = "Error: " + result.Message;
            return Page();
        }

    }
}
