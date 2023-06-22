namespace DreamsWebApp.ViewModels.CategoryVM;
public class CreateCategoryVM
{
	public string Name { get; set; } = null!;
	public IFormFile Image { get; set; } = null!;
	public string? ImageName { get; set; }
}
