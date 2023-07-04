using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.LevelVM;
using DreamsWebApp.ViewModels.SlideVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class LevelController : Controller
{
    private readonly DreamsDataContext _dataContext;
    public LevelController(DreamsDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        List<Level> levels= _dataContext.Levels.ToList();
        return View(levels);
    }

	//DETAIL
	public IActionResult Detail(int id)
	{
		Level? level = _dataContext.Levels.Find(id);
		if (level == null) return NotFound();

		return View(level);
	}


	//UPDATE
	public IActionResult Update(int id)
	{
		Level? level = _dataContext.Levels.Find(id);
		if (level == null) return NotFound();

		UpdateLevelVM levelVM = new()
		{
			Name= level.Name,
		};

		return View(levelVM);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, UpdateLevelVM updateLevel)
	{
		if (!ModelState.IsValid) return View(updateLevel);

		Level? level = _dataContext.Levels.FirstOrDefault(s => s.Id == id);
		if (level == null) return NotFound();

		level.Id = id;
		level.Name = updateLevel.Name;

		_dataContext.Levels.Update(level);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}

	//CREATE
	public IActionResult Create()
	{
		return View();
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CreateLevelVM createLevel)
	{
		if (!ModelState.IsValid) return View(createLevel);

		Level level = new()
		{
			Name = createLevel.Name,
		};

		_dataContext.Levels.Add(level);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}


	//DELETE
	[HttpPost]
	public IActionResult Delete(int id)
	{
		Level? level = _dataContext.Levels.FirstOrDefault(s => s.Id == id);
		if (level == null) return NotFound();

		_dataContext.Levels.Remove(level);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
}
