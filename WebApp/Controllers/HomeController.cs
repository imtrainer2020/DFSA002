using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;
using DBO.IServices;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRoleService service;
        public HomeController(IUserRoleService _service)
        {
            this.service = _service;
        }
        public IActionResult Index()
        {
            var list = this.service.GetAllRolesAsync().Result.Data;
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
