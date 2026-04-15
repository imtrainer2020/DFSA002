using DBO.IServices;
using DBO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.Pages.Shared;

namespace WebAppRazor.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly IUserService userService;
        public IList<User> Users { get; set; }

        [TempData]
        public string? Message { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }
        public AdminDashboardModel(IUserService _userService)
        {
            this.userService = _userService;
        }

        public async Task OnGetAsync()
        {
            var result = await userService.GetAllUsersAsync();
            if (result.IsSuccess)
            {
                Users = result.Data ?? new List<User>();
            }

            var status = TempData["StatusMessage"] as string;
            if (!string.IsNullOrEmpty(status))
            {
                Message = status;
            }

        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int userId)
        {
            // Security check: Prevent Admin from deleting themselves
            var loggedInId = int.Parse(User.FindFirst("DBUserId")?.Value ?? "0");
            if (userId == loggedInId)
            {
                TempData["ErrorMessage"] = "You cannot delete your own admin account.";
                return RedirectToPage();
            }

            var result = await userService.DeleteUserAsync(userId);
            if (result.IsSuccess)
                Message = "User and associated details deleted successfully.";
            else
                TempData["ErrorMessage"] = result.Message;

            return RedirectToPage();
        }

    }
}
