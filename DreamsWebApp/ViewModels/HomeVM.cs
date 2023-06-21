using DreamsWebApp.Models;

namespace DreamsWebApp.ViewModels;
public class HomeVM
{
    public List<Slide> Slides { get; set; }
    public List<Category> Categories { get; set; }
    public List<Widget> Widgets { get; set; }
}
