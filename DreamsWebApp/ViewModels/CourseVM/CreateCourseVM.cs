namespace DreamsWebApp.ViewModels.CourseVM;
public class CreateCourseVM
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public string Section { get; set; } = null!;
    public string Lecture { get; set; } = null!;
    public string Article { get; set; }
    public string LectureDescription { get; set; }
    public string Requirements { get; set; }
    public double Price { get; set; }
}
