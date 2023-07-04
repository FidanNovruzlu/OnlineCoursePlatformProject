namespace DreamsWebApp.Models;
public class Course
{
    public int Id { get; set; }
    public string Description { get; set; }=null!;
    public string ImageName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public string? Requirements { get; set; } 
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
    public int LevelId { get; set; }
    public Level? Level { get; set; }
    public bool? TrendingCourse { get; set; }
    public bool? FeaturedCourse { get; set; }
    public double Rating { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Section> Sections { get; set; }
}