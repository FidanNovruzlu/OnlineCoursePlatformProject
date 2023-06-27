namespace DreamsWebApp.Models;
public class Lecture
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public int SectionId { get; set; }
    public Section Section { get; set; }
}
