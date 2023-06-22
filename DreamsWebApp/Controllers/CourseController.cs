//using DreamsWebApp.DAL;
//using DreamsWebApp.Models;
//using DreamsWebApp.ViewModels.CourseVM;
//using DreamsWebApp.ViewModels.WidgetVM;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;

//namespace DreamsWebApp.Controllers;
//public class CourseController : Controller
//{
//    private readonly DreamsDataContext _dreamsDataContext;
//    public CourseController()
//    {

//    }
//    public IActionResult Create()
//    {
//        return View();
//    }
//    [HttpPost]
//    [ValidateAntiForgeryToken]

//    public async Task<IActionResult> Create(CreateCourseVM createCourseVM)
//    {
//        if(!ModelState.IsValid)
//        {
//            return View(createCourseVM);
//        }

//        if (!createCourseVM.Image.CheckType("image/") & createCourseVM.Image.CheckSize(2048))
//        {
//            ModelState.AddModelError("", "Incorrect image type or size.");
//            return View(createCourseVM);
//        }
//        string newFilename = await createCourseVM.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "img");

//        Widget widget = new()
//        {
//            Count = createWidgetVM.Count,
//            Name = createWidgetVM.Name,
//            ImageName = newFilename
//        };

//        _dataContext.Widgets.Add(widget);
//        _dataContext.SaveChanges();
//        return RedirectToAction(nameof(Index));
//    }
//}