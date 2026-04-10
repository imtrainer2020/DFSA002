using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace WebAppRazor.Pages.Shared
{
    public static class CommonFuncs
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static async Task LogoutAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
