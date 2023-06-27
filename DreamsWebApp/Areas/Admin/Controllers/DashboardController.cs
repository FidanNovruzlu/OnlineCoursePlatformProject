using DreamsWebApp.Controllers;
using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly DreamsDataContext _dataContext;
    public DashboardController(DreamsDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        List<Instructor> instructors= _dataContext.Instructors.ToList();
        List<Course> courses= _dataContext.Courses.ToList();
        List<Student> students= _dataContext.Students.ToList();

       HomeVM vm = new()
       {
           Instructors=instructors,
           Courses=courses,
           Students=students,
       };
        return View(vm);
    }
}
