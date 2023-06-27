using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CourseVM;
using DreamsWebApp.ViewModels.WidgetVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DreamsWebApp.Controllers;
public class CourseController : Controller
{
    private readonly DreamsDataContext _dreamsDataContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CourseController(DreamsDataContext dreamsData, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _dreamsDataContext = dreamsData;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        List<Course> courses = _dreamsDataContext.Courses.ToList();
        return View(courses);
    }
    public IActionResult Grid()
    {
        List<Course> courses = _dreamsDataContext.Courses.ToList();
        return View(courses);
    }
    public IActionResult Detail(int id)
    {
        Course? course = _dreamsDataContext.Courses.Include(i => i.Instructor.Job).Include(c=>c.Category).FirstOrDefault(c=>c.Id==id);
        if(course==null) return NotFound();

        DetailCourseVM detailCourseVM = new()
        {
          
            ImageName= course.ImageName,
            VideoUrl= course.VideoUrl,
            Requirements= course.Requirements,
        };
        return View(detailCourseVM);
    }
    //[Authorize(Roles = "Instructor")]
    public IActionResult Create()
    {
        CreateCourseVM createCourseVM = new()
        {
            Levels = _dreamsDataContext.Levels.ToList(),
            Categories = _dreamsDataContext.Categories.ToList(),
        };
        return View(createCourseVM);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCourseVM createCourseVM)
    {
        if (!ModelState.IsValid)
        {
            return View(createCourseVM);
        }
        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if (!createCourseVM.Image.CheckType("image/") & createCourseVM.Image.CheckSize(2048))
        {
            ModelState.AddModelError("", "Incorrect image type or size.");
            return View(createCourseVM);
        }
        if(!createCourseVM.Video.CheckType("video/") & createCourseVM.Video.CheckSize(4096))
        {
            ModelState.AddModelError("", "Incorrect video type or size!");
            return View(createCourseVM);    
        }
        string newFilename = await createCourseVM.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "img");
        string newVideoFilename = await createCourseVM.Video.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "img");
        
        Course course = new()
        {
            Name = createCourseVM.Name,
            Title = createCourseVM.Title,
            Description = createCourseVM.Description,
            Requirements = createCourseVM.Requirements,
            VideoUrl =newVideoFilename,
            Price=createCourseVM.Price,
            ImageName=newFilename,
            CategoryId=createCourseVM.CategoryId,
            LevelId=createCourseVM.LevelId,
            
        };

        _dreamsDataContext.Courses.Add(course);
        await _dreamsDataContext.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(string commentMessage, int courseId)
    {
        AppUser user = null;
        if (User.Identity.IsAuthenticated)
        {
            user = await _userManager.FindByNameAsync(User.Identity.Name);
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }

        Comment comment = new()
        {
            CreatedDate = DateTime.Now,
            AppUserId= user.Id,
            CourseId= courseId,
            Message= commentMessage,
        };
      
        _dreamsDataContext.Comments.Add(comment);
        _dreamsDataContext.SaveChanges();
        return RedirectToAction("Detail", new { id = courseId });
    }

    //public async Task<IActionResult> DeleteComment(int id)
    //{
    //    var comment = _context.Comments.FirstOrDefault(b => b.Id == id);
    //    _context.Comments.Remove(comment);
    //    _context.SaveChanges();
    //    return RedirectToAction("detail", new { id = comment.BlogId });
    //}

}