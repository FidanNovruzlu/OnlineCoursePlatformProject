namespace DreamsWebApp.Models;
public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }=null!;
    public string ImageName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public string Section { get; set; } = null!;
    public string Lecture { get; set; } 
    public string Article { get; set; } 
    public string LectureDescription { get; set; }
    public string Requirements { get; set; } 
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
    public int LevelId { get; set; }
    public Level Level { get; set; }
}