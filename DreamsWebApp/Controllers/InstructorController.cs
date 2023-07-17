using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DreamsWebApp.Controllers;
public class InstructorController : Controller
{
    private readonly DreamsDataContext _dataContext;
    public InstructorController(DreamsDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Dashboard()
    {
        return View();
    }
    public IActionResult List()
    {
        List<Instructor> instructors = _dataContext.Instructors.ToList();
        return View(instructors);
    }
    public IActionResult Grid()
    {
        return View();
    }
    public IActionResult Course()
    {
        return View();
    }
    public IActionResult Reviews()
    {
        return View();
    }
}
