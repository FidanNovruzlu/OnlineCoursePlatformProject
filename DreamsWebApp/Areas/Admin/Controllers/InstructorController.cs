using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class InstructorController : Controller
{
	private readonly DreamsDataContext _dataContext;
	public InstructorController(DreamsDataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public IActionResult Index(int page = 1, int take = 4)
	{
		List<Instructor> instructors = _dataContext.Instructors.Skip((page - 1) * take).Take(take).Include(c=>c.Courses).ToList();

		int allPageCount = _dataContext.Instructors.Count();

		PaginationVM<Instructor> paginationVM = new()
		{
			CurrentPage = page,
			Instructors = instructors,
			TotalPage = (int)(Math.Ceiling((double)allPageCount / take))
		};

		return View(paginationVM);
	}


	//DETAIL
	public IActionResult Detail(string id)
	{
		Instructor? instructor = _dataContext.Instructors.Include(c=>c.Courses).FirstOrDefault(i => i.Id == id);
		if (instructor == null) return NotFound();

		return View(instructor);
	}
}
