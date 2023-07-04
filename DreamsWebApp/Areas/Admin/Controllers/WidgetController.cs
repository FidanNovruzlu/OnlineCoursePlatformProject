using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.SlideVM;
using DreamsWebApp.ViewModels.WidgetVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class WidgetController : Controller
{
	private readonly DreamsDataContext _dataContext;
	private readonly IWebHostEnvironment _webHostEnvironment;
	public WidgetController(DreamsDataContext dreamsDataContext, IWebHostEnvironment webHostEnvironment)
	{
		_dataContext = dreamsDataContext;
		_webHostEnvironment = webHostEnvironment;
	}
	public IActionResult Index()
	{
		List<Widget> widgets= _dataContext.Widgets.Take(4).ToList();
		return View(widgets);
	}

	//DETAIL
	public IActionResult Detail(int id)
	{
		Widget? widget= _dataContext.Widgets.FirstOrDefault(s => s.Id == id);
		if (widget == null) return NotFound();

		return View(widget);
	}


	//UPDATE
	public IActionResult Update(int id)
	{
		Widget? widget = _dataContext.Widgets.FirstOrDefault(w=>w.Id== id);
		if (widget == null) return NotFound();

		UpdateWidgetWM updateWidgetWM=new ()
		{
			Name=widget.Name,
			ImageName=widget.ImageName,
		};
		return View(updateWidgetWM);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, UpdateWidgetWM update)
	{
		if (!ModelState.IsValid) return View(update);

		Widget? widget = _dataContext.Widgets.FirstOrDefault(s => s.Id == id);
		if (widget == null) return NotFound();

		if (update.Image != null)
		{
			if (!update.Image.CheckType("image/") & update.Image.CheckSize(2048))
			{
				ModelState.AddModelError("", "Incorrect image type or size.");
				return View(update);
			}

			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", widget.ImageName);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			string newFilename = await update.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "img");
			widget.ImageName = newFilename;
		}

		widget.Id = id;
		widget.Name = update.Name;

		_dataContext.Widgets.Update(widget);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
}