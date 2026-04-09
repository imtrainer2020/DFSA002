using DBO.IServices;
using DBO.Models;
using DBO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazor.Pages
{
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

    }
}
