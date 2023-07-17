using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamsWebApp.ViewComponents;

public class SearchBarViewComponent : ViewComponent
{
    private readonly DreamsDataContext _dataContext;
	public SearchBarViewComponent(DreamsDataContext dataContext)
	{
		_dataContext= dataContext;
	}
    public async Task<IViewComponentResult> InvokeAsync()
    {
        List<Course> courses = await _dataContext.Courses.Include(c=>c.Category).ToListAsync();
        return View(courses);
    }
}