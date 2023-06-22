using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels;
public class HomeVM
{
    public int CategoryId { get; set; }
    public List<Slide> Slides { get; set; }
    public List<Category> Categories { get; set; }
    public List<Widget> Widgets { get; set; }
    public List<Master> Masters { get; set; }
    public List<Instructor> Instructors { get; set; }
}
