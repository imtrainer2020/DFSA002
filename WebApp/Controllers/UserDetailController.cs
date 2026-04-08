using DBO.IServices;
using DBO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class UserDetailController : Controller
    {
        private readonly IUserDetailService service;
        public UserDetailController(IUserDetailService _service)
        {
            this.service = _service;
        }

        // GET: UserDetailController
        public ActionResult Index()
        {
            return View();
        }

        // POST: UserDetailController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDetailVM vm)
        {
            try
            {
                // 1. Map ViewModel to Entity Model
                var userDetail = new UserDetail
                {
                    UserId = 1,
                    Address = vm.Address,
                    City = vm.City
                };

                if (vm.Photo == null || vm.Photo.Length == 0)
                {
                    userDetail.Photo = null; // or set to a default image byte[] if you have one
                }
                else
                {
                    // 2. Convert IFormFile to byte[]
                    using var memoryStream = new MemoryStream();
                    await vm.Photo.CopyToAsync(memoryStream);
                    
                    userDetail.Photo = memoryStream.ToArray(); // This is the byte[] EF expects
                }

                // 3. Call Service
                var result = await service.AddUserDetailAsync(userDetail);

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = result.Error;
                // View("Index", vm);

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("Index");
            }
        }

    }
}
