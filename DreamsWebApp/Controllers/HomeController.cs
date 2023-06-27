using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DreamsWebApp.Controllers;
public class HomeController : Controller
{
    private readonly DreamsDataContext _context;
    public HomeController(DreamsDataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Slide> slides =await  _context.Slides.ToListAsync();
        List<Widget> widgets=await _context.Widgets.ToListAsync();
        List<Category> categories = await _context.Categories.Include(c=>c.Instructors).ToListAsync();
        List<Master> masters= await _context.Masters.ToListAsync();
        List<Instructor> instructors= await _context.Instructors.Include(c=>c.Courses).Include(j=>j.Job).ToListAsync();
        List<Course> courses = await _context.Courses.Include(c => c.Instructor).ToListAsync();
        List<Student> students= await _context.Students.ToListAsync();

         HomeVM homeVM = new()
        {
            Slides= slides,
            Widgets=widgets,
            Categories=categories,
            Masters=masters,
            Instructors=instructors,
            Students=students,
            Courses=courses,
        };

        return View(homeVM);
    }

    public IActionResult Search(string searchQuery, int categoryId)
    {
        return View();
    }
}