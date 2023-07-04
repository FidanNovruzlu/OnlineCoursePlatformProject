using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels;
public class HomeVM
{
    public int CourseId { get; set; }
    public int CategoryId { get; set; }
    public List<Slide>? Slides { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Widget>? Widgets { get; set; }
    public List<Master>? Masters { get; set; }
    public List<Instructor>? Instructors { get; set; }
    public List<Course>? Courses { get; set; }
    public List<Student>? Students { get; set; }
    public List<Company>? Companies { get; set; }
    public List<Knowledge>? Knowledges { get; set; }
    public int? Order { get; set; }
    public string? Search { get; set; }
    public string? serachText { get; set; }
    public List<Comment>? Comments { get; set; }
}
