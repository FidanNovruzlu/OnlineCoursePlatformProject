using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.SettingVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SettingController : Controller
{
    private readonly DreamsDataContext _dataContext;
    public SettingController(DreamsDataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public IActionResult Index()
    {
        List<Setting> settings = _dataContext.Settings.ToList();
        return View(settings);
    }


    public IActionResult Update(int id)
    {
        Setting? setting = _dataContext.Settings.FirstOrDefault(x => x.Id == id);
        if (setting == null) return NotFound();

        UpdateSettingVM updateSettingVM = new UpdateSettingVM()
        {
            Value = setting.Value,
        };
        return View(updateSettingVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id, UpdateSettingVM updateSettingVM)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSettingVM);
        }

        Setting? setting = _dataContext.Settings.FirstOrDefault(x => x.Id == id);
        if (setting == null) return NotFound();

        setting.Value = updateSettingVM.Value;

        _dataContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
