using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels;
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


    public IActionResult Index(int page = 1, int take = 4)
    {
        List<Course> courses = _dataContext.Courses.Include(i=>i.Instructor).Include(l=>l.Level).Include(c=>c.Category).ToList();
		int allPageCount = _dataContext.Courses.Count();

		PaginationVM<Course> paginationVM = new()
		{
			CurrentPage = page,
			Courses = courses,
			TotalPage = (int)(Math.Ceiling((double)allPageCount / take))
		};
		return View(paginationVM);
    }

	//DETAIL
	public IActionResult Detail(int id)
	{
		Course? course = _dataContext.Courses.Include(i => i.Instructor)
											 .Include(l => l.Level)
											 .Include(c => c.Category)
											 .Include(s=>s.Sections).FirstOrDefault(category => category.Id == id);

		if (course == null) return NotFound();
		return View(course);
	}
}
