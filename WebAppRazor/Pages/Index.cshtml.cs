using DBO.IServices;
using DBO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.Pages.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebAppRazor.Pages.Shared;

namespace WebAppRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService userService;

        public IndexModel(IUserService _service)
        {
            this.userService = _service;
        }

        // This will keep track of which tab to show
        [BindProperty]
        public bool IsLoginTabActive { get; set; } = true;

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
            IsLoginTabActive = true;

            // Validate input presence
            if (LoginData == null ||
                string.IsNullOrWhiteSpace(LoginData.Email) ||
                string.IsNullOrWhiteSpace(LoginData.Password))
            {
                ErrorMessage = "Please provide both email and password.";
                return Page();
            }

            var result = await userService.GetUserByEmailAsync(LoginData.Email);

            if (!result.IsSuccess || result.Data == null)
            {
                ErrorMessage = result.Error ?? "Invalid email or password.";
                return Page();
            }

            var user = result.Data;

            // Check password match
            if (user.Password == LoginData.Password)
            {
                // 1. Create "Claims" (Information about the user)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email), 
                    new Claim(ClaimTypes.Role, user.Role), // This is the most important part!
                    new Claim("DBUserId", user.UserId.ToString()) //Will use for checking which user is logged in
                };

                // 2. Create the Identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 3. Sign the user in (Give the browser the cookie)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));


                if (!string.IsNullOrEmpty(user.Role) && user.Role.ToLower() == "admin")
                    return RedirectToPage("/AdminDashboard");
                else
                    return RedirectToPage("/Dashboard");
            }

            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        // HANDLER 2: Handles the Register Form
        public async Task<IActionResult> OnPostRegisterAsync()
        {
            IsLoginTabActive = false;

            // Validate input presence
            if (RegisterData == null ||
                string.IsNullOrWhiteSpace(RegisterData.Email) ||
                string.IsNullOrWhiteSpace(RegisterData.Password))
            {
                ErrorMessage = "Please provide both email and password for registration.";
                return Page();
            }

            // Check if user exists
            var check = await userService.IsEmailExistAsync(RegisterData.Email);

            if (check.IsSuccess || check.Data)
            {
                // service failed to determine existence
                ErrorMessage = (check.Error != null && check.Error.Length > 0) ? check.Error : "Email already exists.";
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
                IsLoginTabActive = true;
                Message = "Registration successful! You can now login.";
                return RedirectToPage(); // Refresh page to show message
            }

            ErrorMessage = result.Error ?? "Something went wrong.";
            return Page();
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}
