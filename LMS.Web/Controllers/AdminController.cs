using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult LeaveTypes()
        {
            return View();
        }

        public IActionResult Holidays()
        {
            return View();
        }

        public IActionResult LeaveRequests()
        {
            return View();
        }
    }
}
