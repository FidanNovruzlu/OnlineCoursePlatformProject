using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace DreamsWebApp.Controllers;
public class AccountController:Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM register)
    {
        if(!ModelState.IsValid) return View(register);

        AppUser newUser = new()
        {
            Email = register.Email,
            UserName= register.UserName,
            Name=register.Name,
            Surname=register.Surname
        };

        IdentityResult identityResult =await _userManager.CreateAsync(newUser,register.Password);
        if (!identityResult.Succeeded)
        {
            foreach(IdentityError? error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(register);
            }
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        string link = Url.Action("ConfrimUser", "Account", new { email = register.Email, token = token }, HttpContext.Request.Scheme);

        MailMessage message = new MailMessage("7L2P4QW@code.edu.az", newUser.Email)
        {
            Subject = "Confrimation email",
            Body = $"<a href = \"{link}\"> Click to confrim email.</a>",
            IsBodyHtml = true
        };

        SmtpClient smtpClient = new()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential("7L2P4QW@code.edu.az", "pmretojfjscqqjrk")
        };
        smtpClient.Send(message);
        return RedirectToAction(nameof(Login));
    }
    public async Task<IActionResult> ConfrimUser(string email, string token)
    {
        AppUser user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Incorrect confrim");
            return RedirectToAction("Index", "Home");
        }

        await _signInManager.SignInAsync(user, true);
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>Login(LoginVM login)
    {
        if (!ModelState.IsValid) return View(login);

        AppUser user = await _userManager.FindByNameAsync(login.UserName);
        if (user == null)
        {
            ModelState.AddModelError("", "Incorrect username or password.");
            return View(login);
        }

        Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("", "Incorrect username or password.");
            return View(login);
        }

        await _signInManager.CanSignInAsync(user);

        return RedirectToAction("Index","Home");
    }
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
