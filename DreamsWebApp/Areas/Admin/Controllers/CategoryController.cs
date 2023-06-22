using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.WidgetVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin")]
public class CategoryController : Controller
{
    private readonly DreamsDataContext _dataContext;
    private readonly IWebHostEnvironment _environment;
    public CategoryController(DreamsDataContext dreamsDataContext, IWebHostEnvironment webHostEnvironment)
    {
        _dataContext= dreamsDataContext;
        _environment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        List<Category> categories=_dataContext.Categories.Include(i=>i.Instructors).ToList();
        return View(categories);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryVM createCategoryVM)
    {
        if (!ModelState.IsValid) return View(createCategoryVM);

        if (!createCategoryVM.Image.CheckType("image/") & createCategoryVM.Image.CheckSize(2048))
        {
			ModelState.AddModelError("", "Incorrect image type or size.");
            return View(createCategoryVM);
        }
        string newFilename = await createCategoryVM.Image.UplaodAsync(_environment.WebRootPath, "assets", "img");

		Category category = new()
		{
			Name = createCategoryVM.Name,
			ImageName = newFilename
		};

		_dataContext.Categories.Add(category);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
}
