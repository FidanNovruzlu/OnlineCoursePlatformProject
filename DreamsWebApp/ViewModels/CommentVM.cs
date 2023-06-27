using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels;

public class CommentVM
{
    public string Message { get; set; } = null!;
    public DateTime? CreatedDate { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public Course Course { get; set; }
}
