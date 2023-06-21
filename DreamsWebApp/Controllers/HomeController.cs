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

        HomeVM homeVM = new HomeVM()
        {
            Slides= slides,
            Widgets=widgets,
            Categories=categories
        };

        return View(homeVM);
    }
}