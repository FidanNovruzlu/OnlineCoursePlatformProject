using Microsoft.AspNetCore.Mvc;

namespace DreamsWebApp.Controllers;
public class CategoryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
