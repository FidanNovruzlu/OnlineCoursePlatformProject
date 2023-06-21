namespace DreamsWebApp.ViewModels.WidgetVM;
public class CreateWidgetVM
{
	public string Name { get; set; } = null!;
	public int Count { get; set; }
	public string? ImageName { get; set; } 
	public IFormFile Image { get; set; }=null!;
}
