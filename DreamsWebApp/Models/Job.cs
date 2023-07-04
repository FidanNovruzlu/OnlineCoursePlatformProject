namespace DreamsWebApp.Models;
public class Job
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Instructor>? Instructors { get; set; }
}
