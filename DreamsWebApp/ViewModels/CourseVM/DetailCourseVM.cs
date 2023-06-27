using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels.CourseVM;
public class DetailCourseVM
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageName { get; set; }
    public string Title { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public string Section { get; set; } = null!;
    public string Lecture { get; set; } = null!;
    public string? LectureDescription { get; set; }
    public string? Requirements { get; set; }
    public decimal Price { get; set; }
     public Category Category { get; set; }
    public Level Levels { get; set; }
    public Instructor Instructor { get; set; }
    public CommentVM CommentVM { get; set; }
}
