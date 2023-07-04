using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CourseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Query;
using DreamsWebApp.ViewModels;
using Stripe;

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

    //ALL COURSE
    public async Task< IActionResult> Index(string? search,int? order,int? categoryId,string? instructorId)
    {
        IQueryable<Course> query =  _dreamsDataContext.Courses.Include(i=>i.Instructor).AsQueryable();

        switch (order)
        {
            case 1:
                query = query.OrderBy(p => p.Title);
                break;
            case 2:
                query = query.OrderBy(p => p.Price);
                break;
            case 3:
                query = query.OrderByDescending(p => p.Id);
                break;
			default:
				query = query.OrderBy(p => p.Title); 
				break;
		}

        if (!string.IsNullOrEmpty(search))
        {
            query=query.Where(p=>p.Title.ToLower().Contains(search.ToLower()));  
        }

        if (categoryId != null)
        {
            query=query.Where(c=>c.CategoryId== categoryId);
        }

        if(instructorId != null)
        {
            query = query.Where(i => i.InstructorId == instructorId);
        }

        CourseVM courseVM = new()
        {
            Categories= await _dreamsDataContext.Categories.Include(c=>c.Courses).ToListAsync(),
            Courses= await query.ToListAsync(),
            Instructors=await _dreamsDataContext.Instructors.ToListAsync(),
            Order=order,
            CategoryId= categoryId,
            InstructorId=instructorId,
            Search= search
        };

        return View(courseVM);
    }


    //SEARCH
    public IActionResult Search(string title="")
    {
        IQueryable<Course> query = _dreamsDataContext.Courses.Include(i => i.Instructor).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => p.Title.ToLower().Contains(title.ToLower()));
        }

        CourseVM course = new()
        {
            Categories = _dreamsDataContext.Categories.Include(c => c.Courses).ToList(),
             Courses= query.ToList(),
             Instructors=_dreamsDataContext.Instructors.ToList()
         };

        return View(viewName: "Index",course);
    }



    //DETAIL
    public IActionResult Detail(int id)
    {
        Course? course = _dreamsDataContext.Courses.Include(i => i.Instructor.Job).Include(c=>c.Category).FirstOrDefault(c=>c.Id==id);
        if(course==null) return NotFound();

        //DetailCourseVM detailCourseVM = new()
        //{
        //    ImageName= course.ImageName,
        //    VideoUrl= course.VideoUrl,
        //    Requirements= course.Requirements,
        //    Price= course.Price,
        //    Description= course.Description,
        //    Title= course.Title,
        //};
        return View(course);
    }



    // COMMENT
    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> AddComment(string message, int courseId)
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

        Comment comment = new();
        comment.CreatedDate = DateTime.Now;
        comment.AppUserId = user.Id;
        comment.CourseId = courseId;
        comment.Message = message;

        _dreamsDataContext.Comments.Add(comment);
        _dreamsDataContext.SaveChanges();
        return RedirectToAction("Detail", new { id = courseId });
    }



    //CREATE

    [Authorize(Roles = "Instructor")]
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
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Create(CreateCourseVM createCourseVM)
    {
        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (!ModelState.IsValid)
        {
            createCourseVM.Categories = await _dreamsDataContext.Categories.ToListAsync();
            createCourseVM.Levels = await _dreamsDataContext.Levels.ToListAsync();
            return View(createCourseVM);
        }

        if (!createCourseVM.Image.CheckType("image/") & !createCourseVM.Image.CheckSize(2048))
        {
            ModelState.AddModelError("", "Incorrect image type or size!");
            createCourseVM.Categories = await _dreamsDataContext.Categories.ToListAsync();
            createCourseVM.Levels = await _dreamsDataContext.Levels.ToListAsync();
            return View(createCourseVM);
        }

        if (!createCourseVM.Video.CheckType("video") & !createCourseVM.Video.CheckSize(71680))
        {
            ModelState.AddModelError("", "Incorrect video type or size!");
            createCourseVM.Categories = await _dreamsDataContext.Categories.ToListAsync();
            createCourseVM.Levels = await _dreamsDataContext.Levels.ToListAsync();
            return View(createCourseVM);
        }

        string newFilename = await createCourseVM.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "img");
        string newVideoFilename = await createCourseVM.Video.UplaodAsync(_webHostEnvironment.WebRootPath, "assets", "videos");

        Course newCourse = new()
        {
            Title = createCourseVM.Title,
            Description = createCourseVM.Description,
            Requirements = createCourseVM.Requirements,
            VideoUrl = newVideoFilename,
            Price = createCourseVM.Price,
            ImageName = newFilename,
            CategoryId = createCourseVM.CategoryId,
            LevelId = createCourseVM.LevelId,
            InstructorId = user.Id,
            //Sections = createCourseVM.Sections,
        };

        _dreamsDataContext.Courses.Add(newCourse);
        await _dreamsDataContext.SaveChangesAsync();
        return RedirectToAction("Index", "Course");
    }



    public IActionResult Grid()
    {
        List<Course> courses = _dreamsDataContext.Courses.ToList();
        return View(courses);
    }
 



    //ADD BASKET
    public async Task<IActionResult> AddCart(int? id)
    {
        
        if (id == null || id < 1) return BadRequest();

        Course course = await _dreamsDataContext.Courses.FirstOrDefaultAsync(p => p.Id == id);
        if (course == null) return NotFound();

        string? value = HttpContext.Request.Cookies["basket"];
        List<BasketItemVM> cartsCookies = new List<BasketItemVM>();
        if (value == null)
        {
            HttpContext
                .Response
                .Cookies
                .Append("basket", System.Text.Json.JsonSerializer.Serialize(cartsCookies));
        }
        else
        {
            cartsCookies = System.Text.Json.JsonSerializer.Deserialize<List<BasketItemVM>>(value);
        }

        BasketItemVM? cart = cartsCookies.Find(c => c.Id == id);
        if (cart == null)
        {
            cartsCookies.Add(new BasketItemVM()
            {
                Id=course.Id,
                Count = 1,
                Title = course.Title,
                Price = course.Price,
                ImageName = course.ImageName,
            });
        }
        else
        {
            cart.Count += 1;
        }

        HttpContext.Response.Cookies.Append("basket", System.Text.Json.JsonSerializer.Serialize(cartsCookies), new CookieOptions()
        {
            MaxAge = TimeSpan.FromDays(25)
        });
        return RedirectToAction("Index", "Home");
    }

    //GET BASKET
    public async Task<IActionResult> GetCarts()
    {
        List<Course> courseList = new List<Course>();
        List<BasketItemVM> basketItems = new List<BasketItemVM>();

        string value = HttpContext.Request.Cookies["basket"];
        if (value is null)
        {
            basketItems = null;
        }
        else
        {
            basketItems = System.Text.Json.JsonSerializer.Deserialize<List<BasketItemVM>>(value);
            foreach (var item in basketItems)
            {
                Course course = await _dreamsDataContext.Courses.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == item.Id);
                courseList.Add(course);
            }
        }
   
        return View(basketItems);
    }

    //REMOVE CART
    public async Task<IActionResult> RemoveCart(int id)
    {
        string? value = HttpContext.Request.Cookies["basket"];
        if (value == null) return NotFound();
        else
        {
            List<CartVM>? cartVM = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(value);
            CartVM? cart = cartVM.FirstOrDefault(c => c.Id == id);
            if (cart is not null)
            {
                cartVM.Remove(cart);
            }
            HttpContext.Response.Cookies.Append("basket", System.Text.Json.JsonSerializer.Serialize(cartVM), new CookieOptions()
            {
                MaxAge = TimeSpan.FromMinutes(10)
            });
        }
        return RedirectToAction("GetCarts", "Course");
    }
   
  
   
    public async Task<IActionResult> AddWishList(int id)
    {
        Course? course = await _dreamsDataContext
            .Courses
            .Include(x => x.Category)
            .FirstAsync(x => x.Id == id);
        if (course == null)
        {
            return NotFound();
        }
        string? value = HttpContext.Request.Cookies["WishList"];
        List<CartVM> cartsCookies = new List<CartVM>();
        if (value == null)
        {
            HttpContext
                .Response
                .Cookies
                .Append("WishList", System.Text.Json.JsonSerializer.Serialize(cartsCookies));
        }
        else
        {
            cartsCookies = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(value);
        }

        CartVM? cart = cartsCookies.Find(c => c.Id == id);
        if (cart == null)
        {
            cartsCookies.Add(new CartVM()
            {
                Id = id,
                Count = 1,
                Title = course.Title,
                Price =(double) course.Price,
                CategoryName = course.Category.Name,
                ImageUrl = course.ImageName,
            });
        }
        else
        {
            cart.Count += 1;
        }

        HttpContext.Response.Cookies.Append("WishList", System.Text.Json.JsonSerializer.Serialize(cartsCookies), new CookieOptions()
        {
            MaxAge = TimeSpan.FromDays(25)
        });
        return RedirectToAction("Index", "Home");
    }
  
    public async Task<IActionResult> GetWishList()
    {
        List<Course> courseList = new List<Course>();
        List<CartVM> cartVM = new List<CartVM>();
        string value = HttpContext.Request.Cookies["WishList"];
        if (value is null)
        {
            cartVM = null;
        }
        else
        {

            cartVM = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(value);
            foreach (var item in cartVM)
            {
                Course? course = await _dreamsDataContext.Courses.Include(c => c.Category).FirstOrDefaultAsync();
                courseList.Add(course);
            }
        }
        return View(cartVM);
    }
    public async Task<IActionResult> CheckoutWishList()
    {
        List<Course> courseList = new List<Course>();
        List<CartVM> cartVM = new List<CartVM>();
        string value = HttpContext.Request.Cookies["WishList"];
        if (value is null)
        {
            cartVM = null;
        }
        else
        {

            cartVM = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(value);
            foreach (var item in cartVM)
            {
                Course? course = await _dreamsDataContext.Courses.Include(c => c.Category).FirstOrDefaultAsync();
                courseList.Add(course);
            }
        }
        return View(cartVM);
    }
    public async Task<IActionResult> RemoveCartWishList(int id)
    {
        string? value = HttpContext.Request.Cookies["WishList"];
        if (value == null) return NotFound();
        else
        {
            List<CartVM>? cartVM = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(value);
            CartVM? cart = cartVM.FirstOrDefault(c => c.Id == id);
            if (cart is not null)
            {
                cartVM.Remove(cart);
            }
            HttpContext.Response.Cookies.Append("WishList", System.Text.Json.JsonSerializer.Serialize(cartVM), new CookieOptions()
            {
                MaxAge = TimeSpan.FromMinutes(10)
            });
        }
        return RedirectToAction("GetWishList", "Course");
    }

}