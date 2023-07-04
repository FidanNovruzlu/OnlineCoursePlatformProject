using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels.CourseVM;
public class CreateCourseVM
{
    public string Description { get; set; } = null!;
    public string? ImageName { get; set; }
    public IFormFile Image { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public IFormFile Video { get; set; } = null!;
    public string? Requirements { get; set; }
    public decimal Price { get; set; }
    public List<string>? Sections { get; set; }
    public List<string> Lectures { get; set; }
    public int CategoryId { get; set; }
    public List<Category>? Categories { get; set; }
    public int LevelId { get; set; }
    public List<Level>? Levels { get; set;}
}