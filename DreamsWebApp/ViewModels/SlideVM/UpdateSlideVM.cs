namespace DreamsWebApp.ViewModels.SlideVM;
public class UpdateSlideVM
{
	public int? Rating { get; set; }
	public int? AverageRateing { get; set; }
	public string? SupTitle { get; set; } 
	public string? Title { get; set; } 
	public string? Description { get; set; } 
	public string? StartContent { get; set; }
	public string? EndContent { get; set; } 
	public IFormFile? Image { get; set; }
	public string? ImageName { get; set; } 
}