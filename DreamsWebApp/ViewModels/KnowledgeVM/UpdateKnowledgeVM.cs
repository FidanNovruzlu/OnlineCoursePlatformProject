namespace DreamsWebApp.ViewModels.KnowledgeVM;

public class UpdateKnowledgeVM
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public string? ImageName { get; set; }
}
