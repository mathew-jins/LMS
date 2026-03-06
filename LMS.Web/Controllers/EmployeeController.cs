using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult ApplyLeave()
        {
            return View();
        }

        public IActionResult LeaveHistory()
        {
            return View();
        }
    }
}
