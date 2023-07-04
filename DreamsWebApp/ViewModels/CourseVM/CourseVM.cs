using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels.CourseVM;
public class CourseVM
{
    public int? Order { get; set; }
    public int? CategoryId { get; set; }
    public string? Search { get; set; }
    public string? InstructorId { get; set; }
    public List<Course>? Courses { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Instructor>? Instructors { get; set; }
}
