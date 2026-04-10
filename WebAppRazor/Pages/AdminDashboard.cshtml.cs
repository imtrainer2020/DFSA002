using DBO.IServices;
using DBO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazor.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly IUserService userService;
        public IList<User> Users { get; set; }
        public AdminDashboardModel(IUserService _userService)
        {
            this.userService = _userService;
        }

        public async Task OnGetAsync()
        {
            var result = await userService.GetAllUsersAsync();
            if (result.IsSuccess)
            {
                Users = result.Data;
            }
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }

    }
}
