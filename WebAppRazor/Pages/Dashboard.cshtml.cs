using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.Pages.Shared;

namespace WebAppRazor.Pages
{
    [Authorize(Roles = "User")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await CommonFuncs.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
