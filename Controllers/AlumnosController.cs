using Microsoft.AspNetCore.Mvc;

namespace MvcCoreSASAlumnos.Controllers
{
    public class AlumnosController : Controller
    {

        public AlumnosController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
