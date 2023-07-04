using DreamsWebApp.DAL;
using DreamsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamsWebApp.ViewComponents;
public class FooterViewComponent : ViewComponent
{
	private readonly DreamsDataContext _dataContext;
	public FooterViewComponent(DreamsDataContext dataContext)
	{
		_dataContext = dataContext;
	}
	public async Task<IViewComponentResult> InvokeAsync()
	{
		Dictionary<string, Setting> settings = await _dataContext.Settings.ToDictionaryAsync(k => k.Key);
		return View(settings);
	}
}
