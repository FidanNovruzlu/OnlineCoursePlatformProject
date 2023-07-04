namespace DreamsWebApp.Models;
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ImageName { get; set; } = null!;
    public List<Instructor>? Instructors { get; set; }
    public List<Course> Courses { get;}
}