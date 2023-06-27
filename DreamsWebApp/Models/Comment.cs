namespace DreamsWebApp.Models;
public class Comment
{
    public int Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime? CreatedDate { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public int CourseId { get; set; }
    public Course Course { get; set;}
}