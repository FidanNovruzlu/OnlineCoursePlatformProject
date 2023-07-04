using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.MasterVM;
using DreamsWebApp.ViewModels.SlideVM;
using DreamsWebApp.ViewModels.WidgetVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class MasterController : Controller
{
    private readonly DreamsDataContext _dreamsDataContext;
	private readonly IWebHostEnvironment _environment;
    public MasterController(DreamsDataContext dreamsDataContext,IWebHostEnvironment environment)
    {
        _dreamsDataContext = dreamsDataContext;
		_environment = environment;
    }

    public IActionResult Index()
    {
        List<Master> masters=_dreamsDataContext.Masters.Take(4).ToList();
        return View(masters);
    }

	//CREATE
	public IActionResult Create()
	{
		return View();
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CreateMasterVM create)
	{
		if (!ModelState.IsValid) return View(create);

		if (!create.Image.CheckType("image/") & create.Image.CheckSize(2048))
		{
			ModelState.AddModelError("", "Incorrect image type or size.");
			return View(create);
		}
		string newFilename = await create.Image.UplaodAsync(_environment.WebRootPath, "assets", "img","icon");

		Master master = new()
		{
			Description = create.Description,
			ImageName = newFilename
		};

		_dreamsDataContext.Masters.Add(master);
		_dreamsDataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}

	//DETAIL
	public IActionResult Detail(int id)
	{
		Master? master = _dreamsDataContext.Masters.FirstOrDefault(m => m.Id == id);
		if (master == null) return NotFound();

		return View(master);
	}

	//UPDATE
	public IActionResult Update(int id)
	{
		Master? master = _dreamsDataContext.Masters.Find(id);
		if (master == null) return NotFound();

		UpdateMasterVM update = new()
		{
			Description = master.Description,
			ImageName = master.ImageName,
		};

		return View(update);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, UpdateMasterVM update)
	{
		if (!ModelState.IsValid) return View(update);

		Master? master = _dreamsDataContext.Masters.FirstOrDefault(s => s.Id == id);
		if (master == null) return NotFound();

		if (update.Image != null)
		{
			if (!update.Image.CheckType("image/") & update.Image.CheckSize(2048))
			{
				ModelState.AddModelError("", "Incorrect image type or size.");
				return View(update);
			}

			string path = Path.Combine(_environment.WebRootPath, "assets", "img","icon", master.ImageName);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			string newFilename = await update.Image.UplaodAsync(_environment.WebRootPath, "assets", "img","icon");
			master.ImageName = newFilename;
		}

		master.Id = id;
		master.Description = update.Description;

		_dreamsDataContext.Masters.Update(master);
		_dreamsDataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}


	//DELETE
	[HttpPost]
	public IActionResult Delete(int id)
	{
		Master? master = _dreamsDataContext.Masters.FirstOrDefault(s => s.Id == id);
		if (master == null) return NotFound();

		string path = Path.Combine(_environment.WebRootPath, "assets", "img","icon", master.ImageName);
		if (System.IO.File.Exists(path))
		{
			System.IO.File.Delete(path);
		}

		_dreamsDataContext.Masters.Remove(master);
		_dreamsDataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
}
