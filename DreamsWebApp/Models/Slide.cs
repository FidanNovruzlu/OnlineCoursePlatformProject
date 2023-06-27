namespace DreamsWebApp.Models;
public class Slide
{
    public int Id { get; set; }
    public double Rating { get; set; }
    public int AverageRateing { get; set; }
    public string SupTitle { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;    
    public string StartContent { get; set; } = null!;
    public string EndContent { get; set; } = null!;
    public string ImageName { get; set; } = null!;
    public int CatagoryId { get; set; }
    public Category Category { get; set; }
}
