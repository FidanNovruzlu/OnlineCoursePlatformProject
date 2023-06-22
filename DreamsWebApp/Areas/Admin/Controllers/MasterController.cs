using DreamsWebApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DreamsWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class MasterController : Controller
{
    private readonly DreamsDataContext _dreamsDataContext;
    public MasterController(DreamsDataContext dreamsDataContext)
    {
        _dreamsDataContext = dreamsDataContext;
    }

    public IActionResult Index()
    {
        return View();
    }
}
