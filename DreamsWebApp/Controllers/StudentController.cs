using Microsoft.AspNetCore.Mvc;

namespace DreamsWebApp.Controllers;
public class StudentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult List()
    {
        return View();
    }
    public IActionResult Grid()
    {
        return View();
    }
}
