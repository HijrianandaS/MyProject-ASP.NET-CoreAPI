//using Client.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class DepartmentsController : Controller
    {
        //[Authentication]
        public IActionResult Index()
        {
            return View();
        }
    }
}
