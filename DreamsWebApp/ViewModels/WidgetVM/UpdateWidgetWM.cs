namespace DreamsWebApp.ViewModels.WidgetVM;
public class UpdateWidgetWM
{
	public string? Name { get; set; } 
	public int? Count { get; set; }
	public string? ImageName { get; set; }
	public IFormFile? Image { get; set; } 
}