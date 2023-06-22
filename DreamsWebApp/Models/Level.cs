namespace DreamsWebApp.Models;
public class Level
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Course> Courses { get; set; }
}
