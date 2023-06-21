namespace DreamsWebApp.Models;
public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }=null!;
    public string ImageName { get; set; } = null!;
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
}