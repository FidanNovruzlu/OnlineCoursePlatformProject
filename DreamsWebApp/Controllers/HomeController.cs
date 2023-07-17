using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.Services.Interfaces;
using DreamsWebApp.ViewModels;
using DreamsWebApp.ViewModels.CheckoutVM;
using DreamsWebApp.ViewModels.CourseVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DreamsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DreamsDataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public HomeController(DreamsDataContext context, UserManager<AppUser> userManager, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string? search)
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            List<Widget> widgets = await _context.Widgets.ToListAsync();
            List<Category> categories = await _context.Categories.Include(c => c.Instructors).ToListAsync();
            List<Master> masters = await _context.Masters.ToListAsync();
            List<Instructor> instructors = await _context.Instructors.Include(c => c.Courses).Include(j => j.Job).ToListAsync();
            List<Course> courses = await _context.Courses.Take(3).Include(c => c.Instructor).ToListAsync();
            List<Student> students = await _context.Students.ToListAsync();
            List<Company> companies = await _context.Companies.ToListAsync();
            List<Knowledge> knowledges = await _context.Knowledges.ToListAsync();
            List<Comment> comments= await _context.Comments.Include(c=>c.Course).ToListAsync();

            HomeVM homeVM = new()
            {
                Slides = slides,
                Widgets = widgets,
                Categories = categories,
                Masters = masters,
                Instructors = instructors,
                Students = students,
                Courses = courses,
                Companies = companies,
                Knowledges = knowledges,
                Comments = comments,
            };

            return View(homeVM);
        }


        public async Task<IActionResult> Checkout()
        {

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            ViewBag.BasketItems = await _context.BasketItems.Where(b => b.AppUserId == user.Id && b.OrderId == null).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderVM orderVM, string stripeEmail, string stripeToken)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            List<BasketItem> items = await _context.BasketItems
                .Where(b => b.AppUserId == user.Id && b.OrderId == null)
                .Include(b => b.Course)
                .ToListAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.BasketItems = items;
                return View();
            }

            decimal total = 0;
            foreach (var item in items)
            {
                total += item.Count * item.Price;
            }

            Order order = new Order()
            {
                AppUserId = user.Id,
                Status = null,
                BasketItems = items,
                PurchaseAt = DateTime.Now,
                TotalPrice = total,
                Address = orderVM.Address
            };

            // STRIPE

            var optionCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = user.Name + " " + user.Surname,
                Phone = "+994 50 66"
            };
            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionCust);

            long totalAmount = (long)(total * 100); // Convert total amount to cents

            if (totalAmount < 50)
            {
                totalAmount = 50; // Set minimum amount as $0.50 USD
            }

            var optionsCharge = new ChargeCreateOptions
            {
                Amount = totalAmount,
                Currency = "USD",
                Description = "Product Selling amount",
                Source = stripeToken,
                ReceiptEmail = stripeEmail
            };

            var serviceCharge = new ChargeService();
            Charge charge = serviceCharge.Create(optionsCharge);

            if (charge.Status != "succeeded")
            {
                ViewBag.BasketItems = items;
                ModelState.AddModelError("Address", "Ödeme işleminde bir problem oluştu");
                return View();
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            string body = @"
        <table>
            <thead>
                <tr>
                    <th>Course</th>
                    <th>Count</th>
                    <th>Price</th>
                </tr>
            </thead>
            <tbody>";

            foreach (var item in order.BasketItems)
            {
                body += $@"
            <tr>
                <td>{item.Course.Title}</td>
                <td>{item.Count}</td>
                <td>{item.Price}</td>
            </tr>";
            }

            body += @"
            </tbody>
        </table>";

            await _emailService.SendEmail(user.Email, "Order Placement", body, true);

            return RedirectToAction(nameof(Index));
        }
    }
}