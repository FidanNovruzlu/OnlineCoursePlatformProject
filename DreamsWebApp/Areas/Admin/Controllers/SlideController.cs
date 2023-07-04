using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.SlideVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SlideController : Controller
{
    private readonly DreamsDataContext _dataContext;
    private readonly IWebHostEnvironment _environment;
    public SlideController(DreamsDataContext dreamsDataContext,IWebHostEnvironment webHostEnvironment)
    {
        _dataContext= dreamsDataContext;
        _environment= webHostEnvironment;
    }

    public IActionResult Index()
    {
        ICollection<Slide> slides = _dataContext.Slides.ToList();
		return View(slides);
    }

    //DETAIL
    public IActionResult Detail(int id)
    {
        Slide? slide = _dataContext.Slides.Find(id);
        if (slide == null) return NotFound();
        return View(slide);
    }


    //UPDATE
    public IActionResult Update(int id)
    {
        Slide? slide = _dataContext.Slides.Find(id);
        if (slide == null) return NotFound();

        UpdateSlideVM updateSlideVM= new()
        {
            Title= slide.Title,
            SupTitle= slide.SupTitle,
            EndContent  = slide.EndContent,
            StartContent= slide.StartContent,
            Description = slide.Description,
            ImageName= slide.ImageName,
        };

        return View(updateSlideVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id,UpdateSlideVM updateSlideVM) 
    {
        if(!ModelState.IsValid) return View(updateSlideVM);

		Slide? slide = _dataContext.Slides.FirstOrDefault(s => s.Id == id);
		if (slide == null) return NotFound();

        if (updateSlideVM.Image != null)
        {
            if (!updateSlideVM.Image.CheckType("image/") & updateSlideVM.Image.CheckSize(2048))
            { 
				ModelState.AddModelError("", "Incorrect image type or size.");
				return View(updateSlideVM);
			}

			string path = Path.Combine(_environment.WebRootPath, "assets", "img", slide.ImageName);
			if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

			string newFilename = await updateSlideVM.Image.UplaodAsync(_environment.WebRootPath, "assets", "img");
			slide.ImageName = newFilename;
		}

		slide.Id = id;
        slide.StartContent = updateSlideVM.StartContent;
        slide.EndContent = updateSlideVM.EndContent;
        slide.Description = updateSlideVM.Description;
        slide.Title = updateSlideVM.Title;
        slide.SupTitle = updateSlideVM.SupTitle;


        _dataContext.Slides.Update(slide);
		_dataContext.SaveChanges();
		return RedirectToAction(nameof(Index));
    }

}
