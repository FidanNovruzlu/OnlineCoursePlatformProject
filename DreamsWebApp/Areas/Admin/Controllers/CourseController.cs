using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class CourseController : Controller
{
    private readonly DreamsDataContext _dataContext;
    public CourseController(DreamsDataContext dreamsDataContext)
    {
        _dataContext= dreamsDataContext;
    }
    public IActionResult Index()
    {
        List<Course> courses = _dataContext.Courses.Include(c=>c.Category).ToList();
        return View(courses);
    }
}
