using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.SlideVM;
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


    public IActionResult Index(int page = 1, int take = 4)
    {
        List<Category> categories=_dataContext.Categories.Skip((page - 1) * take).Take(take).Include(i=>i.Instructors).ToList();

		int allPageCount = _dataContext.Categories.Count();

		PaginationVM<Category> paginationVM = new()
		{
			CurrentPage = page,
			Categories = categories,
			TotalPage = (int)(Math.Ceiling((double)allPageCount / take))
		};

		return View(paginationVM);
    }

	//DETAIL
	public IActionResult Detail(int id)
	{
		Category? category = _dataContext.Categories.Include(i=>i.Instructors).FirstOrDefault(category=>category.Id==id);
		if (category == null) return NotFound();

		return View(category);
	}

	//CREATE
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


	//UPDATE
	public IActionResult Update(int id)
	{
		Category? category = _dataContext.Categories.Find(id);
		if (category == null) return NotFound();

		UpdateCategoryVM updateCategoryVM = new()
		{
			Name = category.Name,
			ImageName = category.ImageName,
		};

		return View(updateCategoryVM);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, UpdateCategoryVM updateCategory)
	{
		if (!ModelState.IsValid) return View(updateCategory);

		Category? category = _dataContext.Categories.FirstOrDefault(s => s.Id == id);
		if (category == null) return NotFound();

		if (updateCategory.Image != null)
		{
			if (!updateCategory.Image.CheckType("image/") & updateCategory.Image.CheckSize(2048))
			{
				ModelState.AddModelError("", "Incorrect image type or size.");
				return View(updateCategory);
			}

			string path = Path.Combine(_environment.WebRootPath, "assets", "img", category.ImageName);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			string newFilename = await updateCategory.Image.UplaodAsync(_environment.WebRootPath, "assets", "img");
			category.ImageName = newFilename;
		}

		category.Id = id;
		category.Name = updateCategory.Name;

		_dataContext.Categories.Update(category);
		_dataContext.SaveChanges();

		return RedirectToAction(nameof(Index));
	}

	//DELETE
	[HttpPost]
	public  IActionResult Delete(int id)
	{
		Category? category = _dataContext.Categories.FirstOrDefault(s => s.Id == id);
		if (category == null) return NotFound();

		string path = Path.Combine(_environment.WebRootPath,"assets", "img", category.ImageName);
		if (System.IO.File.Exists(path))
		{
			System.IO.File.Delete(path);
		}

		_dataContext.Categories.Remove(category);
		 _dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
}
