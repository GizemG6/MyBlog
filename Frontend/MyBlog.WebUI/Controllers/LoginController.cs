using Microsoft.AspNetCore.Mvc;

namespace MyBlog.WebUI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
