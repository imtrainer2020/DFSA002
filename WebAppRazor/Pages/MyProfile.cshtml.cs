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
        private readonly IUserService userService;

        public MyProfileModel(IUserDetailService _service, IUserService _userService)
        {
            this.service = _service;
            this.userService = _userService;
        }

        // Helper to get ID from Claim
        [BindProperty]
        public int LoggedInUserId => int.Parse(User?.FindFirst("DBUserId")?.Value ?? "0");

        public string DashboardLink => User.IsInRole("Admin") ? "/AdminDashboard" : "/Dashboard";

        [BindProperty]
        public UserProfileInput ProfileInput { get; set; }

        [BindProperty]
        public IFormFile? UploadedPhoto { get; set; }

        [BindProperty]
        public int TargetUserId { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string? StatusMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? userId)
        {
            if (LoggedInUserId == 0) return RedirectToPage("/Index");

            // If Admin is editing someone else, use that ID, otherwise use own ID
            TargetUserId = (userId.HasValue && User.IsInRole("Admin")) ? userId.Value : LoggedInUserId;

            ProfileInput = new UserProfileInput { UserId = TargetUserId };
            var userDetail = await service.GetUserDetailByUserIdAsync(TargetUserId);

            if (userDetail != null && userDetail.IsSuccess && userDetail.Data != null)
            {
                var data = userDetail.Data;
                ProfileInput.Udid = data.Udid;
                ProfileInput.FullName = data.FullName;
                ProfileInput.Address = data.Address;
                ProfileInput.City = data.City;
                ProfileInput.PostCode = data.PostCode;
                ProfileInput.Phone = data.Phone;
                ProfileInput.Photo = data.Photo;
            }

            return Page();
        }

        // Form 1: Save Profile Details
        public async Task<IActionResult> OnPostAsync()
        {
            UserDetail dbRecord = new UserDetail
            {
                UserId = TargetUserId,
                FullName = ProfileInput.FullName,
                Address = ProfileInput.Address,
                City = ProfileInput.City,
                PostCode = ProfileInput.PostCode,
                Phone = ProfileInput.Phone,
                Udid = ProfileInput.Udid
            };

            if (UploadedPhoto != null)
            {
                using (var ms = new MemoryStream())
                {
                    await UploadedPhoto.CopyToAsync(ms);
                    dbRecord.Photo = ms.ToArray();
                }
            }
            else
                dbRecord.Photo = ProfileInput.Photo;

            var existing = await service.GetUserDetailByUserIdAsync(TargetUserId);
            if (existing == null || !existing.IsSuccess)
                await service.AddUserDetailAsync(dbRecord);
            else
                await service.UpdateUserDetailAsync(dbRecord);

            TempData["StatusMessage"] = "Profile updated successfully!";
            return RedirectToPage(new { userId = TargetUserId });
        }

        // Form 2: Change Password (Named Handler)
        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!User.IsInRole("Admin"))
            {
                ErrorMessage = "Only Admin can change passwords";
                return RedirectToPage("/Dashboard");
            }
            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match or are empty.";
                await OnGetAsync(TargetUserId);
                return Page();
            }

            var userResult = await userService.GetUserByUserIdAsync(TargetUserId);
            if (!userResult.IsSuccess)
            {
                ErrorMessage = "User not found.";
                return Page();
            }

            var result = await userService.ResetPasswordAsync(NewPassword, userResult.Data.Email);
            if (result.IsSuccess)
            {
                TempData["StatusMessage"] = "Password has been updated successfully.";
                return RedirectToPage(new { userId = TargetUserId });
            }

            ErrorMessage = result.Message;
            await OnGetAsync(TargetUserId);
            return Page();
        }
    }
}