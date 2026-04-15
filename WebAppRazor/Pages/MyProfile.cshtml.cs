using DBO.IServices;
using DBO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.Pages.Models;

namespace WebAppRazor.Pages
{
    [Authorize]
    public class MyProfileModel : PageModel
    {
        private readonly IUserDetailService service;

        int LoggedInUserId => int.Parse(User?.FindFirst("DBUserId")?.Value ?? "0");

        // This property helps the HTML know where to send the "Back" button
        public string DashboardLink => User.IsInRole("Admin") ? "/AdminDashboard" : "/Dashboard";

        public MyProfileModel(IUserDetailService _service)
        {
            this.service = _service;
        }

        [BindProperty]
        public UserProfileInput ProfileInput { get; set; }

        [BindProperty]
        public IFormFile? UploadedPhoto { get; set; }

        public string? StatusMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Get the logged-in User's ID from the Claims
            if (LoggedInUserId == 0)
                return RedirectToPage("/Index");

            // 2. Fetch details from DB
            ProfileInput = new UserProfileInput { UserId = LoggedInUserId };

            var userDetail = await service.GetUserDetailByUserIdAsync(LoggedInUserId);
            if (userDetail != null && userDetail.IsSuccess && userDetail.Data != null)
            {
                var data = userDetail.Data;
                ProfileInput.FullName = data.FullName;
                ProfileInput.Address = data.Address;
                ProfileInput.City = data.City;
                ProfileInput.PostCode = data.PostCode;
                ProfileInput.Phone = data.Phone;
                ProfileInput.Photo = data.Photo;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Ensure the UserId is correctly set
            ProfileInput.UserId = LoggedInUserId;

            UserDetail dbRecord = new UserDetail
            {
                UserId = ProfileInput.UserId,
                FullName = ProfileInput.FullName,
                Address = ProfileInput.Address,
                City = ProfileInput.City,
                PostCode = ProfileInput.PostCode,
                Phone = ProfileInput.Phone
            };

            // 2. Handle the Photo Upload if a new file is selected
            if (UploadedPhoto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await UploadedPhoto.CopyToAsync(memoryStream);
                    ProfileInput.Photo = memoryStream.ToArray();
                }
            }

            dbRecord.Photo = ProfileInput.Photo;

            // 4. Update or Add
            var exisingRecord = await service.GetUserDetailByUserIdAsync(ProfileInput.UserId);

            if (exisingRecord== null || !exisingRecord.IsSuccess)
            {
                await service.AddUserDetailAsync(dbRecord);
                StatusMessage = "Profile created successfully!";
            }
            else
            {
                await service.UpdateUserDetailAsync(dbRecord);
                StatusMessage = "Profile updated successfully!";
            }

            return RedirectToPage(DashboardLink);
        }
    }
}
