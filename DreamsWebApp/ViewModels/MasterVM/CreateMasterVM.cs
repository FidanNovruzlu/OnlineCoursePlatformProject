namespace DreamsWebApp.ViewModels.MasterVM;
public class CreateMasterVM
{
	public string Description { get; set; } = null!;
	public IFormFile Image { get; set; } = null!;
	public string? ImageName { get; set; }
}
