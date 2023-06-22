using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
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
	public IActionResult Create()
	{
		return View();
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CreateWidgetVM createWidgetVM)
	{
		if (!ModelState.IsValid) return View(createWidgetVM);

		if (!createWidgetVM.Image.CheckType("image/") & createWidgetVM.Image.CheckSize(2048))
		{
			ModelState.AddModelError("", "Incorrect image type or size.");
			return View(createWidgetVM);
		}
		string newFilename = await createWidgetVM.Image.UplaodAsync(_webHostEnvironment.WebRootPath,"assets", "img");

		Widget widget = new()
		{
			Count=createWidgetVM.Count,
			Name=createWidgetVM.Name,
			ImageName=newFilename
		};

		_dataContext.Widgets.Add(widget);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
	public IActionResult Detail(int id)
	{
		Widget? widget= _dataContext.Widgets.FirstOrDefault(s => s.Id == id);
		if (widget == null) return NotFound();

		return View(widget);
	}
	public IActionResult Update(int id)
	{
		Widget? widget = _dataContext.Widgets.FirstOrDefault(w=>w.Id== id);
		if (widget == null) return NotFound();
		UpdateWidgetWM updateWidgetWM=new UpdateWidgetWM()
		{
			Count=widget.Count,
			Name=widget.Name,
			ImageName=widget.ImageName,
		};
		return View(updateWidgetWM);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, UpdateWidgetWM updateWidgetWM)
	{
		if (!ModelState.IsValid) return View(updateWidgetWM);

		Widget? widget=  await _dataContext.Widgets.FirstOrDefaultAsync(s => s.Id == id);
		if (widget == null) return NotFound();


		if (!updateWidgetWM.Image.CheckType("image/") & updateWidgetWM.Image.CheckSize(2048))
		{
			ModelState.AddModelError("", "Incorrect image type or size.");
			return View(updateWidgetWM);
		}
		if (updateWidgetWM.Image != null)
		{
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", widget.ImageName);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
			string newFilename = await updateWidgetWM.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "img");

			widget.ImageName = newFilename;
		}

		_dataContext.Widgets.Update(widget);
		 await _dataContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	[HttpPost]
	public IActionResult Delete(int id)
	{
		Widget? widget = _dataContext.Widgets.FirstOrDefault(s => s.Id == id);
		if (widget == null) return NotFound();

		string path = Path.Combine(_webHostEnvironment.WebRootPath,"assets", "img", widget.ImageName);
		if (System.IO.File.Exists(path))
		{
			System.IO.File.Delete(path);
		}
		_dataContext.Widgets.Remove(widget);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
	}
}
