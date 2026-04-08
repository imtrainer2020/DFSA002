using DBO.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class UserRoleController : Controller
    {
        IUserRoleService service;
        public UserRoleController(IUserRoleService _service)
        {
            this.service = _service;
        }
        public IActionResult Index()
        {
            var list = this.service.GetAllRolesAsync().Result;
            return View();
        }
    }
}
