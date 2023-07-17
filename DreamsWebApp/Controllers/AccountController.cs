using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using NuGet.Common;
using DreamsWebApp.Services.Interfaces;
using DreamsWebApp.ViewModels.MailSenderVM;

namespace DreamsWebApp.Controllers;
public class AccountController:Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IEmailService _emailService;

    public AccountController(UserManager<AppUser> userManager,
							SignInManager<AppUser> signInManager,
							RoleManager<IdentityRole> roleManager,
							IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
		_emailService = emailService;
    }


	//Register
	public IActionResult InstructorRegister()
	{
		return View();
	}
	public IActionResult StudentRegister()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> InstructorRegister(InstructorRegisterVM register)
	{
		if (!ModelState.IsValid) return View();

		Instructor newUser = new()
		{
			Name = register.Name,
			UserName = register.UserName,
			Email = register.Email,
            Surname= register.Surname,
			IsTeacher = true
		};

		IdentityResult identityResult = await _userManager.CreateAsync(newUser, register.Password);

		if (!identityResult.Succeeded)
		{
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(register);
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
		await _userManager.AddToRoleAsync(newUser, "Instructor");
		await _signInManager.SignInAsync(newUser, true);
		return RedirectToAction(nameof(Login));
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> StudentRegister(StudentRegisterVM register)
	{
		if (!ModelState.IsValid) return View();

		AppUser newUser = new()
		{
			Name = register.Name,
			Surname = register.Surname,
			UserName = register.UserName,
			Email = register.Email
		};

		IdentityResult identityResult = await _userManager.CreateAsync(newUser, register.Password);

		if (!identityResult.Succeeded)
		{
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(register);
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
		await _userManager.AddToRoleAsync(newUser, "Student");
		//await _userManager.AddToRoleAsync(newUser, "Admin");
		await _signInManager.SignInAsync(newUser, true);
		return RedirectToAction(nameof(Login));
	}




	//Confrimation
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



	//Login

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

		if (signInResult.IsLockedOut)
		{
			ModelState.AddModelError("", "The account is locked Out");
			return View(login);
		}

        return RedirectToAction("Index","Home");
    }



	//Log out
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }



	//Role
	public async Task CreateRole()
	{
		if (!await _roleManager.RoleExistsAsync("Admin"))
		{
			await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
		}
		if (!await _roleManager.RoleExistsAsync("Instructor"))
		{
			await _roleManager.CreateAsync(new IdentityRole { Name = "Instructor" });
		}
		if (!await _roleManager.RoleExistsAsync("Student"))
		{
			await _roleManager.CreateAsync(new IdentityRole { Name = "Student" });
		}
	}



	//Forgot Password
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
    {
        if (!ModelState.IsValid) return NotFound();

        AppUser exsistUser = await _userManager.FindByEmailAsync(forgotPassword.Email);

        if (exsistUser is null)
        {
            ModelState.AddModelError("Email", "Email isn't found");
            return View();
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);
        string link =   Url.Action(nameof(ResetPassword), "Account", new { userId = exsistUser.Id, token=token },HttpContext.Request.Scheme);

		await _emailService.SendEmailAsync(new MailRequestVM { ToEmail = forgotPassword.Email, Subject = "ResetPassword", Body = $"<a href=\"{link}\">Reset Password</a>" });

        return RedirectToAction(nameof(Login));

    }



	//Reset Password
    public async Task<IActionResult> ResetPassword(string userId, string token)
    {
		if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();

		AppUser user= await _userManager.FindByIdAsync(userId);
		if(user is null) return NotFound();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword, string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return BadRequest();

        if (!ModelState.IsValid)
            return View(resetPassword);

        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return NotFound();

        var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(resetPassword);
        }

        return RedirectToAction(nameof(Login));
    }
}