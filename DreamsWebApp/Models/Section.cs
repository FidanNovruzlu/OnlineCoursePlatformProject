namespace DreamsWebApp.Models;
public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public List<Lecture> Lectures { get; set; }
}
