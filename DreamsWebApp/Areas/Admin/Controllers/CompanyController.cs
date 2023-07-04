using DreamsWebApp.DAL;
using DreamsWebApp.Extensions;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels;
using DreamsWebApp.ViewModels.CategoryVM;
using DreamsWebApp.ViewModels.CompanyVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CompanyController : Controller
{
    private readonly DreamsDataContext _dataContext;
    private readonly IWebHostEnvironment _environment;
    public CompanyController(DreamsDataContext dataContext, IWebHostEnvironment environment)
    {
        _dataContext = dataContext;
        _environment = environment;
    }


    public IActionResult Index(int page = 1, int take = 5)
    {
        List<Company> companies= _dataContext.Companies.Skip((page - 1) * take).Take(take).ToList();
		int allPageCount = _dataContext.Companies.Count();

		PaginationVM<Company> paginationVM = new()
		{
			CurrentPage = page,
			Companies = companies,
			TotalPage = (int)(Math.Ceiling((double)allPageCount / take))
		};
		return View(paginationVM);
    }

    //DETAIL
    public IActionResult Detail(int id)
    {
        Company? company = _dataContext.Companies.FirstOrDefault(c => c.Id == id);
        if (company == null) return NotFound();

        return View(company);
    }

    //CREATE
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCompanyVM createCompany)
    {
        if (!ModelState.IsValid) return View(createCompany);

        if (!createCompany.Image.CheckType("image/") & createCompany.Image.CheckSize(2048))
        {
            ModelState.AddModelError("", "Incorrect image type or size.");
            return View(createCompany);
        }
        string newFilename = await createCompany.Image.UplaodAsync(_environment.WebRootPath, "assets", "img");

        Company company = new()
        {
            ImageName = newFilename
        };

        _dataContext.Companies.Add(company);
        _dataContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


    //UPDATE
    public IActionResult Update(int id)
    {
        Company? company = _dataContext.Companies.Find(id);
        if (company == null) return NotFound();

        UpdateCompanyVM updateCompany = new()
        {
            ImageName = company.ImageName,
        };

        return View(updateCompany);
    }
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, UpdateCompanyVM updateCompany)
	{
		if (!ModelState.IsValid) return View(updateCompany);

		Company? company = _dataContext.Companies.FirstOrDefault(s => s.Id == id);
		if (company == null) return NotFound();

		if (updateCompany.Image != null)
		{
			if (!updateCompany.Image.CheckType("image/") & updateCompany.Image.CheckSize(2048))
			{
				ModelState.AddModelError("", "Incorrect image type or size.");
				return View(updateCompany);
			}

			string path = Path.Combine(_environment.WebRootPath, "assets", "img", company.ImageName);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			string newFilename = await updateCompany.Image.UplaodAsync(_environment.WebRootPath, "assets", "img");
			company.ImageName = newFilename;
		}

		company.Id = id;

		_dataContext.Companies.Update(company);
		_dataContext.SaveChanges();

		return RedirectToAction(nameof(Index));
	}


	//DELETE
	[HttpPost]
    public IActionResult Delete(int id)
    {
        Company? company = _dataContext.Companies.FirstOrDefault(s => s.Id == id);
        if (company == null) return NotFound();

        string path = Path.Combine(_environment.WebRootPath, "assets", "img", company.ImageName);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        _dataContext.Companies.Remove(company);
        _dataContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
