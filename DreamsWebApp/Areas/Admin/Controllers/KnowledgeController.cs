using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.KnowledgeVM;
using DreamsWebApp.ViewModels.SlideVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class KnowledgeController : Controller
{
    private readonly DreamsDataContext _dataContext;
    private readonly IWebHostEnvironment _environment;
    public KnowledgeController(DreamsDataContext dreamsDataContext, IWebHostEnvironment webHostEnvironment)
    {
        _dataContext = dreamsDataContext;
        _environment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        ICollection<Knowledge> knowledges = _dataContext.Knowledges.ToList();
        return View(knowledges);
    }

    //DETAIL
    public IActionResult Detail(int id)
    {
        Knowledge? knowledge = _dataContext.Knowledges.Find(id);
        if (knowledge == null) return NotFound();

        return View(knowledge);
    }


    //UPDATE
    public IActionResult Update(int id)
    {
        Knowledge? knowledge = _dataContext.Knowledges.Find(id);
        if (knowledge == null) return NotFound();

        UpdateKnowledgeVM update = new()
        {
            Title = knowledge.Title,
            Description = knowledge.Description,
            ImageName = knowledge.ImageName,
        };

        return View(update);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateKnowledgeVM update)
    {
        if (!ModelState.IsValid) return View(update);

        Knowledge? knowledge = _dataContext.Knowledges.FirstOrDefault(s => s.Id == id);
        if (knowledge == null) return NotFound();

        if (update.Image != null)
        {
            if (!update.Image.CheckType("image/") & update.Image.CheckSize(2048))
            {
                ModelState.AddModelError("", "Incorrect image type or size.");
                return View(update);
            }

            string path = Path.Combine(_environment.WebRootPath, "assets", "img", knowledge.ImageName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            string newFilename = await update.Image.UplaodAsync(_environment.WebRootPath, "assets", "img");
            knowledge.ImageName = newFilename;
        }

        knowledge.Id = id;
        knowledge.Description = update.Description;
        knowledge.Title = update.Title;

        _dataContext.Knowledges.Update(knowledge);
        _dataContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
